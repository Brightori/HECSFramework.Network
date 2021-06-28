using HECSFramework.Core;
using MessagePack;
using System;
using System.Collections.Generic;

namespace Commands
{
    [MessagePackObject]
    public struct DeltaSliceNetworkCommand : INetworkCommand
    {
        [Key(0)]
        public int CurrentSliceIndex;

        [Key(1)]
        public List<Guid> CurrentEntities;
    }
}