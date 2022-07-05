using HECSFramework.Core;
using HECSFramework.Network;
using System;

namespace Components
{
    [Serializable]
    public partial class LocationTagComponent : BaseComponent, INetworkComponent, IAfterSerializationComponent, IWorldSingleComponent
    {
        [Field(0)]
        [HideInInspectorCrossPlatform]
        public int LocationZone;
        public bool IsDirty { get; set; }
        
        [Field(1)] 
        public int Version { get; set; }

        [Field(2)]
        [HideInInspectorCrossPlatform]
        public string LocationAssetGuid;

        public void AfterSync()
        {
            IsDirty = true;
        }
    }
}