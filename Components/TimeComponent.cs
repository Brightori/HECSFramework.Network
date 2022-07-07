using HECSFramework.Core;
using System;

namespace Components
{
    public class TimeComponent : BaseComponent, IWorldSingleComponent
    {
        private long serverTickMilliseconds;
        private long startSyncMillis, startLocalTime;
        private uint currentTick;

        public TimeComponent() { }
        public void Start(long currentMillis, long serverTickMilliseconds)
        {
            startSyncMillis = currentMillis;
            startLocalTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.serverTickMilliseconds = serverTickMilliseconds;
            currentTick = (uint)(currentMillis / serverTickMilliseconds);
            DeltaTime = serverTickMilliseconds / 1000f;
        }


        public long ServerTickMilliseconds => serverTickMilliseconds;

        /// <summary>Current tick</summary>
        public long TickCount => currentTick;
        public long CurrentMillis => startSyncMillis + (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startLocalTime);
        public DateTime CurrentTime => DateTimeOffset.FromUnixTimeMilliseconds(CurrentMillis).DateTime;

        /// <summary>Time elapsed since tick change</summary>
        public long TimeAfterTick => CurrentMillis - (currentTick * serverTickMilliseconds);

        /// <summary>Time to next tick</summary>
        public long TimeUntilTick => serverTickMilliseconds - TimeAfterTick;

        public float DeltaTime { get; private set; }
       

        public void NextTick() => currentTick++;
    }
}
