using Commands;
using HECSFramework.Core;

namespace HECSFramework.Network
{
    public class HECSDataProcessor : IDataProcessor
    {
        void IDataProcessor.Process(ResolverDataContainer message)
        {
            switch (message.Type)
            {
                case 0:
                    TypesMap.GetComponentInfo(message.TypeHashCode, out var info);
                    if (!EntityManager.TryGetEntityByID(message.EntityGuid, out var entity))
                    {
                        HECSDebug.LogWarning($"Receiving entity not found: {message.EntityGuid}, component: {info.ComponentName}");
                        break;
                    }

                    if (entity.GetAllComponents[info.ComponentsMask.Index] == null)
                        entity.AddHecsComponent(EntityManager.ResolversMap.GetComponentFromContainer(message));
                    else
                        EntityManager.ResolversMap.ProcessResolverContainer(ref message, ref entity);
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