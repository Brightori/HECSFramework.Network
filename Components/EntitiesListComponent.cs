using HECSFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Systems;

namespace Components
{
    public class EntitiesListComponent : BaseComponent
    {
        private Dictionary<int, IEntity> ClientEntities = new Dictionary<int, IEntity>();
        private Dictionary<int, EntityReplicationSystem> replicatedEntities = new Dictionary<int, EntityReplicationSystem>();
    }
}
