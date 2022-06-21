using HECSFramework.Core;
using LiteNetLib;

namespace Commands
{
    public struct ClientDisconnectedCommand : IGlobalCommand
    {
        public DisconnectReason Reason { get; set; }
    }
}