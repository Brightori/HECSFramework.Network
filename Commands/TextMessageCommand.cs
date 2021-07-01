using HECSFramework.Core;

namespace Commands
{
    public struct TextMessageCommand : INetworkCommand, IData
    {
        public string TextMessage;
    }
}