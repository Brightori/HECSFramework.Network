using HECSFramework.Network;

namespace Components
{
    public partial class ActorContainerID : INetworkComponent
    {
        public int Version { get; set; }
        public bool IsDirty { get; set; } 
    }
}