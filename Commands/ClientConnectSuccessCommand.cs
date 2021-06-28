using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePackObject]
    public struct ClientConnectSuccessCommand : INetworkCommand
    {
        [Key(0)]
        public int ServerTickIntervalMilliseconds;
    }
}