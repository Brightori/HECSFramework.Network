using HECSFramework.Core;
using HECSFramework.Network;
using System;

namespace Components
{
    [Serializable]
    public partial class CustomEntityContainerComponent : BaseComponent, INetworkComponent
    {
        [Field(0)]
        public string ContainerGUID;

        public int Version { get; set; }
        public bool IsDirty { get; set; }
    }
}