using HECSFramework.Core;
using HECSFramework.Network;
using System;

namespace Components
{
    [Serializable]
    public class ReplicatedNetworkEntityComponent : BaseComponent, INotReplicable
    {
    }
}