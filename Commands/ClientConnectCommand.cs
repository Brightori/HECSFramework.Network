using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    public struct ClientConnectCommand : INetworkCommand 
    {
        [Key(1)]
        public int Version;

        [Key(2)]
        public int RoomWorld;
    }
}