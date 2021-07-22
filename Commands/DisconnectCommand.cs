using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePack.MessagePackObject]
    public struct DisconnectCommand : INetworkCommand
    {
        [Key(0)]
        public string Reason;
    }
}
