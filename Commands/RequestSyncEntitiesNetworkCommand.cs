using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePackObject]
    public struct RequestSyncEntitiesNetworkCommand : INetworkCommand
    {
        [Key(0)]
        public int Index;
    }
}