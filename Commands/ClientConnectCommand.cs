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

        [Key(1)]
        public int Zone;

        [Key(2)]
        public string Preffix;
        
        [Key(3)]
        public int Version;
        
        [Key(4)]
        public string Suffix;
    }
}