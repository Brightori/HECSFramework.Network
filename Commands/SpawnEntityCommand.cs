using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject, Serializable]
    public struct SpawnEntityCommand : INetworkCommand
    {
        [Key(0)]
        public Guid ClientGuid;

        [Key(1)]
        public Guid CharacterGuid;

        [Key(2)]
        public byte[] Entity;

        [Key(3)]
        public bool IsNeedRecieveConfirm;

        [Key(4)]
        public int Index;
    }
}