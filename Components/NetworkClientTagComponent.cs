using HECSFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    [Documentation("Network", "Компонент который вешает на одну из основных сущностей, и айди сущности будет являться айди этого клиента")]
    public class NetworkClientTagComponent : BaseComponent
    {
        public Guid ClientGuid => Owner.GUID;
    }
}