using System;
using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePackObject]
    public struct ClientConnectSuccessCommand : INetworkCommand, IData
    {
        [Key(0)]
        public ServerData ServerData;

        [Key(1)]
        public Guid Guid;
    }

    [Serializable, MessagePackObject]
    public partial class ServerData
    {
        [Key(0)]
        public int ServerTickIntervalMilliseconds;
 
        [Key(1)]
        public string ConfigData { get; set; }

        [Key(2)]
        public int DisconnectTimeoutMs;
    }
}