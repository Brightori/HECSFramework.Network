using HECSFramework.Core;

namespace Commands
{
    public struct TextMessageCommand : INetworkCommand
    {
        public string TextMessage;
    }
}