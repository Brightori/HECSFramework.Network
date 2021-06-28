using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    public struct AddOrRemoveComponentToClientCommand : INetworkCommand
    {
        [Key(0)]
        public Guid Entity;

        [Key(1)]
        public ResolverDataContainer component;

        [Key(2)]
        public bool IsAdded;
    }
}
