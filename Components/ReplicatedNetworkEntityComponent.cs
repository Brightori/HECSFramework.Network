using HECSFramework.Core;
using HECSFramework.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    [Serializable]
    public class ReplicatedNetworkEntityComponent : BaseComponent, INotReplicable
    {
    }
}
