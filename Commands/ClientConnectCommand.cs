﻿using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    public struct ClientConnectCommand : INetworkCommand, IData
    {
        [Key(0)]
        public Guid Client;

        [Key(1)]
        public int Zone;
    }
}
