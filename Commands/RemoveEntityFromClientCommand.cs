using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    public struct RemoveEntityFromClientCommand : INetworkCommand
    {
        [Key(0)]
        public Guid EntityToRemove;
    }
}