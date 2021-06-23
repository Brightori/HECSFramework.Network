using HECSFramework.Core;
using HECSFramework.Network;
using System;

namespace Components
{
    [Serializable]
    public class LocationComponent : BaseComponent, INetworkComponent, IAfterSerializationComponent
    {
        [Field(0)]
        public int LocationZone;
        public bool IsDirty { get; set; }
        
        [Field(1)] 
        public int Version { get; set; }

        public void AfterSync()
        {
            IsDirty = true;
        }
    }
}