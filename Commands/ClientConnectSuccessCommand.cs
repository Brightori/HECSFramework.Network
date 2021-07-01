using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePackObject]
    public struct ClientConnectSuccessCommand : INetworkCommand, IData
    {
        [Key(0)]
        public int ServerTickIntervalMilliseconds;
    }
}