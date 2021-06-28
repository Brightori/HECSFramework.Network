using HECSFramework.Core;
using System;

namespace Components
{
    [Documentation("Network", "Компонент который вешает на одну из основных сущностей, и айди сущности будет являться айди этого клиента")]
    [Serializable]
    public partial class NetworkClientTagComponent : BaseComponent
    {
        public Guid ClientGuid => Owner.GUID;
        
        [Field(0)]
        public int WorldIndex;
    }
}