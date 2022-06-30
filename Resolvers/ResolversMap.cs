using HECSFramework.Network;
using System;
using System.Collections.Generic;
using Commands;

namespace HECSFramework.Core
{
    public partial class ResolversMap
    {
        private Dictionary<int, ICommandResolver> hashTypeToResolver = new Dictionary<int, ICommandResolver>(64);
        private Dictionary<Type, int> typeTohash = new Dictionary<Type, int>(64);

        public ResolverDataContainer GetCommandContainer<T>(T command, Guid sender) where T : INetworkCommand, IData
        {
            return new ResolverDataContainer
            {
                Data = MessagePack.MessagePackSerializer.Serialize(command),
                Type = 2,
                EntityGuid = sender,
                TypeHashCode = typeTohash[typeof(T)],
            };
        }

        public EntityResolver GetNetworkEntityResolver(IEntity entity)
        {
            var resolverMap = EntityManager.ResolversMap;
            var resolver = new EntityResolver();
            resolver.Components = new List<ResolverDataContainer>(16);
            resolver.Systems = new List<ResolverDataContainer>(16);

            foreach (var c in entity.GetAllComponents)
            {
                if (c == null)
                    continue;

                if (c is INetworkComponent)
                    resolver.Components.Add(resolverMap.GetComponentContainer(c));
            }

            foreach (var s in entity.GetAllSystems)
            {
                if (s == null || s is INotReplicable)
                    continue;

                resolver.Systems.Add(resolverMap.GetSystemContainer(s));
            }
            resolver.Guid = entity.GUID;

            return resolver;
        }

        public void ProcessCommand(ResolverDataContainer resolverDataContainer, World world)
        {
            if (!hashTypeToResolver.TryGetValue(resolverDataContainer.TypeHashCode, out var resolver))
            {
                HECSDebug.LogWarning($"Undefined command: {resolverDataContainer.TypeHashCode}");
                return;
            }

            resolver.ResolveCommand(resolverDataContainer, world);
        }

        public string GetCommandName(ResolverDataContainer resolverDataContainer)
            => hashTypeToResolver.TryGetValue(resolverDataContainer.TypeHashCode, out var resolver) ? resolver.GetCommandName(resolverDataContainer) : string.Empty;
    }

    public class CommandResolver<T> : ICommandResolver where T : struct, INetworkCommand
    {
        public void ResolveCommand(ResolverDataContainer resolverDataContainer, World world)
        {
            var command = MessagePack.MessagePackSerializer.Deserialize<T>(resolverDataContainer.Data);

            if (resolverDataContainer.EntityGuid == Guid.Empty)
                world.Command(command);
            else if (world.TryGetEntityByID(resolverDataContainer.EntityGuid, out var entity))
                    entity.Command(command);
            else
                HECSDebug.LogWarning($"Receiving entity not found: {resolverDataContainer.EntityGuid}");
        }

        public string GetCommandName(ResolverDataContainer resolverDataContainer)
            => MessagePack.MessagePackSerializer.Deserialize<T>(resolverDataContainer.Data).GetType().Name;
    }

    public partial interface ICommandResolver
    {
        void ResolveCommand(ResolverDataContainer resolverDataContainer, World world);
        string GetCommandName(ResolverDataContainer resolverDataContainer);
    }

    public interface INetworkCommand : ICommand, IGlobalCommand, IData
    {
    }
}