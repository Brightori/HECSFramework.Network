using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    public struct RemovedComponentOnServerCommand : INetworkCommand
    {
        [Key(0)]
        public Guid Entity;

        [Key(1)]
        public int TypeIndex;
    }
}
