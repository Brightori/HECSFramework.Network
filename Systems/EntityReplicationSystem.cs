using Components;
using HECSFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{

    [Documentation(Doc.Server, "This system is responsible for synchronizing the components between the server and the client")]
    public class EntityReplicationSystem : BaseSystem, IAfterEntityInit
    {
        private ReplicationComponent[] replicationComponents;

        public short ID { get; internal set; }


        public override void InitSystem()
        {

        }

        public void AfterEntityInit()
        {
            replicationComponents = Owner.GetComponentsByType<ReplicationComponent>().ToArray();
        }

        internal List<byte[]> GetFullComponentsData()
        {
            List<byte[]> components = new List<byte[]>();

            foreach(ReplicationComponent replicationComponent in replicationComponents)
            {
                components.Add(replicationComponent.GetFullData());
            }

            return components;
        }
    }
}
