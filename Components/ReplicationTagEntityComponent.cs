using HECSFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Components
{
    [Documentation(Doc.Network, "Component that marks entities that replicate")]
    public class ReplicationTagEntityComponent : BaseComponent, IAfterEntityInit
    {
        public short ID { get; private set; } = 0;
        private ReplicationComponent[] replicationComponents;

        public void Init(short id)
        {
            if(ID != 0) throw new Exception("Attempting to re-register a replicated entity");
            ID = id;
        }

        public void AfterEntityInit()
        {
            replicationComponents = Owner.GetComponentsByType<ReplicationComponent>().ToArray();
        }

        internal List<byte[]> GetFullComponentsData()
        {
            List<byte[]> components = new List<byte[]>();

            foreach (ReplicationComponent replicationComponent in replicationComponents)
            {
                components.Add(replicationComponent.GetFullData());
            }

            return components;
        }

        internal void UpdateComponent(List<byte[]> components)
        {
            foreach(byte[] componentData in components)
            {
                int componentID = BitConverter.ToInt32(componentData, 0);

                Array.Find(replicationComponents, c => c.ComponentID == componentID).UpdateData(componentData);
            }
        }
    }
}
