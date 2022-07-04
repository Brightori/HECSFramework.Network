using Commands;
using Components;
using HECSFramework.Core;
using HECSFramework.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{

    [Documentation(Doc.Server, "This system is responsible for synchronizing the components between the server and the client")]
    public class EntityReplicationSystem : BaseSystem, IAfterEntityInit, IReactCommand<CreateReplicationEntity>
    {

        private Dictionary<short, ReplicationTagEntityComponent> replicatedEntities = new Dictionary<short, ReplicationTagEntityComponent>();



        public override void InitSystem()
        {

        }

        public void AfterEntityInit()
        {

        }

        public async void CommandReact(CreateReplicationEntity command)
        {
            var container = Owner.World.GetSingleComponent<ContainerProviderComponent>().GetEntityContainer(command.ContainerID);
            var actor = await container.GetActor();
            actor.Init();

            var replicatedEntity = actor.GetHECSComponent<ReplicationTagEntityComponent>();
            replicatedEntity.Init(command.EntityID);
            replicatedEntity.UpdateComponent(command.Components);

            replicatedEntities.Add(command.EntityID, replicatedEntity);
        }
    }
}
