using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    public struct ClientConnectCommand : INetworkCommand
    {
        [Key(0)]
        public Guid Client;
    }
}
