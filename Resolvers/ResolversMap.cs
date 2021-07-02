using System;
using System.Collections.Generic;

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

        public void ProcessCommand(ResolverDataContainer resolverDataContainer)
        {
            if (hashTypeToResolver.TryGetValue(resolverDataContainer.TypeHashCode, out var resolver))
            {
                resolver.ResolveCommand(resolverDataContainer);
            }
        }
    }

    public class CommandResolver<T> : ICommandResolver where T : INetworkCommand
    {
        public void ResolveCommand(ResolverDataContainer resolverDataContainer, int worldIndex = 0)
        {
            var command = MessagePack.MessagePackSerializer.Deserialize<T>(resolverDataContainer.Data);
            EntityManager.Command(command, worldIndex);
        }
    }

    public interface ICommandResolver
    {
        void ResolveCommand(ResolverDataContainer resolverDataContainer, int worldIndex = 0);
    }

    public interface INetworkCommand : ICommand, IGlobalCommand 
    {
    }
}