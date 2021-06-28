using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    public struct RequestEntityFromServerNetworkCommand : INetworkCommand
    {
        [Key(0)]
        public Guid ClientID;

        [Key(1)]
        public Guid NeededEntity;
    }
}