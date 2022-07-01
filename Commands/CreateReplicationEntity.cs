using HECSFramework.Core;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    [Documentation(Doc.Network, "Command to create an entity with component data replication")]
    public struct CreateReplicationEntity : ICommand
    {
        [Key(0)]
        public short EntityID;
        [Key(1)]
        public int ContainerID;
        [Key(2)]
        public List<byte[]> Components;
    }
}
