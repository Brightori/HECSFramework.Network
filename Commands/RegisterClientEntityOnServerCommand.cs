using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    [Documentation("Network", "Мы отправляем на сервер команду с просьбой зарегистроровать данную ентити")]
    public struct RegisterClientEntityOnServerCommand : INetworkCommand
    {
        [Key(0)]
        public Guid ClientGuid;

        [Key(1)]
        public EntityResolver Entity;
    }
}
