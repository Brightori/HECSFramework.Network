using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    public struct SyncClientNetworkCommand : INetworkCommand
    {
        [Key(0)]
        public Guid ClientGuid;

        [Key(1)]
        public int World;
    }
}