using HECSFramework.Core;
using System;

namespace Components
{
    public partial class TimeComponent : BaseComponent, IWorldSingleComponent
    {
        public DateTime CurrentTime => DateTime.UtcNow;
    }
}
