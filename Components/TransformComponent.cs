using HECSFramework.Core;
using HECSFramework.Network;

namespace Components
{
    public partial class TransformComponent : BaseComponent, INetworkComponent, IUnreliable
    {
        public int Version { get; set; }
        public bool IsDirty { get; set; }

        partial void InfoUpdated()
        {
            IsDirty = true;
            Version++;
        }
    }
}
