using HECSFramework.Core;

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
    }
}
