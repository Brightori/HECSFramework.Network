using Commands;
using HECSFramework.Core;
using System.Collections.Concurrent;

namespace Systems
{
    [Documentation(Doc.Server, "The system is responsible for adding, removing entities to the world")]
    public class EntitiesSystem : BaseSystem, IUpdatable
    {
        private short generatorID = 1;
        private ConcurrentQueue<IEntity> incomingEntities = new ConcurrentQueue<IEntity>();

        private ConcurrencyList<IEntity> entityForRep;

      //  private Dictionary<int, IEntity> nonReplicatedEntities = new Dictionary<int, IEntity>();

        private DataSenderSystem dataSender;
        public override void InitSystem()
        {
            entityForRep = Owner.World.Filter(new FilterMask(HMasks.NetworkEntityTagComponent, HMasks.ReplicationDataComponent));
        }

        public bool AddEntity(Guid clientID, IEntity entity)
        {
            incomingEntities.Enqueue(entity);
            dataSender = EntityManager.GetSingleSystem<DataSenderSystem>();

           return Owner.World.TryGetEntityByID(clientID, out var client);
        }

        public void UpdateLocal()
        {
           
            while(incomingEntities.TryDequeue(out IEntity entity))
            {
                if(entity.TryGetSystem(out EntityReplicationSystem entityReplication))
                {
 
                    do { entityReplication.ID = generatorID++; } while (replicatedEntities.ContainsKey(entityReplication.ID));
                    
                    //Send a command to all entities in the world to create this new entity

                    var createCMD = new CreateReplicationEntity()
                    {
                        EntityID = entityReplication.ID,
                        ContainerID = 0,
                        Components = entityReplication.GetFullComponentsData()
                    };

                    dataSender.SendCommand(ClientEntities.Values, createCMD);
                    //Send a command to the new entity to create all the entities that are in the world
         
                    foreach(EntityReplicationSystem e in replicatedEntities.Values)
                    {
                        
                        var c = new CreateReplicationEntity()
                        {
                            EntityID = e.ID,
                            Components = e.GetFullComponentsData()
                        };
                    }
                }
            }
        }
    }
}
