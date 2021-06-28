using HECSFramework.Core;
using HECSFramework.Network;
using System;

namespace Components
{
    [Serializable]
    public partial class NetworkEntityTagComponent : BaseComponent, INetworkComponent
    {
        public bool IsDirty { get; set; }
        [Field(0)] public int Version { get; set; }
    }
}