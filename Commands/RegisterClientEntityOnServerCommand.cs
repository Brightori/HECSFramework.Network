using HECSFramework.Core;
using MessagePack;
using System;

namespace Commands
{
    [MessagePackObject]
    [Documentation("Network", "Мы отправляем на сервер команду с просьбой зарегистроровать данную ентити")]
    ///если нам нужна проектозависимая история - делаем парт часть, там прописываем данные, 
    ///и отдельную систему которая обрабатывает эти данные
    public partial struct RegisterClientEntityOnServerCommand : INetworkCommand
    {
        [Key(0)]
        public Guid ClientGuid;

        [Key(1)]
        public EntityResolver Entity;
    }
}
