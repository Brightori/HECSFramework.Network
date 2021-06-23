using HECSFramework.Core;
using MessagePack;

namespace Commands
{
    [MessagePackObject]
    public struct StopServerCommand : INetworkCommand
    {
    }
}