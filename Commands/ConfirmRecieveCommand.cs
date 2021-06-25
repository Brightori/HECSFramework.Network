using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePackObject]
    public struct ConfirmRecieveCommand : INetworkCommand
    {
        [Key(0)]
        public int Index;
    }
}