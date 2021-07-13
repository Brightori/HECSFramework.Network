using HECSFramework.Network;

namespace Components
{
    public partial class TransformComponent : INetworkComponent, IUnreliable
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
