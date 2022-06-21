using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject, Serializable]
    public struct SpawnEntityCommand : INetworkCommand
    {
        [Key(0)]
        public Guid CharacterGuid;

        [Key(1)]
        public EntityResolver Entity;

        [Key(2)]
        public int WorldId;
    }
}