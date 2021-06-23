using System;
using System.Collections.Generic;
using System.Text;

namespace HECSFramework.Core
{
    public partial class ResolversMap
    {
        private Dictionary<int, ICommandResolver> hashTypeToResolver = new Dictionary<int, ICommandResolver>(64);
        private Dictionary<Type, int> typeTohash = new Dictionary<Type, int>(64);
        partial void InitPartialCommandResolvers();

        public void InitCommandResolvers()
        {
            InitPartialCommandResolvers();
        }

        public ResolverDataContainer GetCommandContainer<T>(T command, Guid sender) where T : INetworkCommand
        {
            return new ResolverDataContainer
            {
                Data = command,
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
            EntityManager.Command((T)resolverDataContainer.Data, worldIndex);
        }
    }

    public interface ICommandResolver
    {
        void ResolveCommand(ResolverDataContainer resolverDataContainer, int worldIndex = 0);
    }


    public interface INetworkCommand : ICommand, IGlobalCommand, IData
    {
    }
}