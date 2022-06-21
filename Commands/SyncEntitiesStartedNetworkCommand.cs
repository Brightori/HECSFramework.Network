using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [Documentation(Doc.Server, "Команда приходит на клиент с сервера при начале синхронизации")]
    [Serializable]
    [MessagePackObject]
    public struct SyncEntitiesStartedNetworkCommand : INetworkCommand
    {
        [Key(0)] public Guid[] Entities { get; set; }
    }
}