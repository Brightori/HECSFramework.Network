using HECSFramework.Core;
using System;
using HECSFramework.Network;

namespace Components
{
    [Documentation("Tag", "Этим тэгом мы помечаем те энтити, которые должны жить на сервере")]
    [Serializable]
    public class ServerEntityTagComponent : BaseComponent, INetworkComponent, IServerSide
    {
        public int Version { get; set; }
        public bool IsDirty { get; set; }
    }
}