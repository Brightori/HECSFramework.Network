using HECSFramework.Core;
using MessagePack;
using System.Collections.Generic;

namespace Commands
{
    [MessagePackObject]
    public struct SyncServerComponentsCommand : INetworkCommand
    {
        [Key(0)]
        public List<ResolverDataContainer> Components;
    }
}