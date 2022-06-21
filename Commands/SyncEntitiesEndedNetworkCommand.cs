using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [Documentation(Doc.Server, "Уведомление на сервер с клиента о конце синхронизации")]
    [Serializable]
    [MessagePackObject]
    public struct SyncEntitiesEndedNetworkCommand : INetworkCommand
    {
        [Key(0)] public Guid Client { get; set; }
    }
}