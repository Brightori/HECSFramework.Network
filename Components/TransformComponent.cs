using HECSFramework.Core;
using HECSFramework.Network;

namespace Components
{
    public sealed partial class TransformComponent : BaseComponent, INetworkComponent, IUnreliable
    {
        public int Version { get; set; }

        public void InfoUpdated()
        {
            IsDirty = true;
            Version++;
        }
    }
}