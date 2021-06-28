using HECSFramework.Core;
using HECSFramework.Network;

namespace Components
{
    public class WorldSliceIndexComponent : BaseComponent, INetworkComponent
    {
        private int index;

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