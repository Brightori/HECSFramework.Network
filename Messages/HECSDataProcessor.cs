﻿using HECSFramework.Core;

namespace HECSFramework.Network
{
    public class HECSDataProcessor : IDataProcessor
    {
        void IDataProcessor.Process(ResolverDataContainer message)
        {
            switch (message.Type)
            {
                case 0:
                    if (EntityManager.TryGetEntityByID(message.EntityGuid, out var entity))
                    {
                        EntityManager.ResolversMap.ProcessResolverContainer(ref message, ref entity);
                    }
                    break;
                case 1:
                    break;
                case 2:
                    EntityManager.ResolversMap.ProcessCommand(message);
                    break;

            }
        }
    }
}

namespace HECSFramework.Network
{
    public interface IDataProcessor
    {
        void Process(ResolverDataContainer message);
    }
}