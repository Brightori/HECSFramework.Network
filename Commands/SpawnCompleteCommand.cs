using HECSFramework.Core;
using MessagePack;
using System.Collections.Generic;

namespace Commands
{
    [MessagePackObject]
    public struct SpawnCompleteCommand : INetworkCommand
    {
        [Key(0)]
        public List<SpawnEntityCommand> SpawnEntities;

        [Key(1)]
        public int SyncIndex;

        [Key(2)]
        public int LocationIndex;
    }
}