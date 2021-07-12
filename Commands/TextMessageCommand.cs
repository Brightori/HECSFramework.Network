using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePack.MessagePackObject]
    public struct TextMessageCommand : INetworkCommand, IData
    {
        [Key(0)]
        public string TextMessage;
    }
}