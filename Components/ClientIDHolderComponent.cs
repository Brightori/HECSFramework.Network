using HECSFramework.Core;
using System;

namespace Components
{
    public partial class ClientIDHolderComponent : BaseComponent
    {
        public Guid ClientID { get; set; }
    }
}
