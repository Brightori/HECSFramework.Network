using System.Collections.Generic;
using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePackObject]
    [Documentation(Doc.Network, "Command to create an entity with component data replication")]
    public struct CreateReplicationEntity : ICommand, INetworkCommand
    {
        [Key(0)]
        public short EntityID;
        [Key(1)]
        public int ContainerID;
        [Key(2)]
        public List<byte[]> Components;
    }
}
