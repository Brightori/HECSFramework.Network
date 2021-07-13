using HECSFramework.Core;
using HECSFramework.Network;
using System;

namespace Components
{
    public class WorldSliceIndexComponent : BaseComponent, INetworkComponent
    {
        private int index;
        public ConcurrencyList<Guid> EntitiesOnClient = new ConcurrencyList<Guid>();
        public ConcurrencyList<Guid> EntitiesToRemove = new ConcurrencyList<Guid>();

        [Field(0)]
        public int Index
        {
            get => index;
            set
            {
                IsDirty = index != value;
                index = value;
            }
        }

        public int Version { get; set; }
        public bool IsDirty { get; set; }
    }
}