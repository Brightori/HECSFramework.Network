using HECSFramework.Network;
using HECSFramework.Unity;
using System.Collections.Generic;

namespace HECSFramework.Core
{
    public partial struct EntityResolver
    {
        public EntityResolver GetServerEntityResolver(IEntity entity)
        {
            Systems = new List<ResolverDataContainer>(32);
            Components = new List<ResolverDataContainer>(32);
            Guid = entity.GUID;

            foreach (var c in entity.GetAllComponents)
            {
                if (c == null)
                    continue;

                if (c is IClientSide)
                    continue;

                if (c is IHaveActor)
                    continue;

                Components.Add(EntityManager.ResolversMap.GetComponentContainer(c));
            }

            foreach (var s in entity.GetAllSystems)
            {
                if (s == null)
                    continue;

                if (s is IClientSide)
                    continue;

                if (s is IHaveActor)
                    continue;

                Systems.Add(EntityManager.ResolversMap.GetSystemContainer(s));
            }

            return this;
        }

        public EntityResolver GetClientEntityResolver(IEntity entity)
        {
            Systems = new List<ResolverDataContainer>(32);
            Components = new List<ResolverDataContainer>(32);
            Guid = entity.GUID;

            foreach (var c in entity.GetAllComponents)
            {
                if (c == null)
                    continue;

                if (c is INotReplicable || c is IServerSide)
                    continue;

                Components.Add(EntityManager.ResolversMap.GetComponentContainer(c));
            }

            foreach (var s in entity.GetAllSystems)
            {
                if (s == null)
                    continue;

                if (s is INotReplicable || s is IServerSide)
                    continue;

                Systems.Add(EntityManager.ResolversMap.GetSystemContainer(s));
            }

            return this;
        }
    }
}