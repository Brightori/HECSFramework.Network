using HECSFramework.Core;
using HECSFramework.Network;

namespace Components
{
    public partial class ViewReferenceComponent : BaseComponent, INetworkComponent
    {
        public int Version { get; set; }
        public bool IsDirty { get; set; }
    }
}