using HECSFramework.Core;
using System;

namespace Components
{
    public partial class TimeComponent : BaseComponent
    {
        public DateTime CurrentTime => DateTime.UtcNow;
    }
}
