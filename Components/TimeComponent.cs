﻿using HECSFramework.Core;
using HECSFramework.Server;

namespace Components
{
    public class TimeComponent : BaseComponent
    {
        private Config config;
        private long currentTick;
        public DateTime CurrentTime => DateTime.UtcNow;

        public TimeComponent() { }
        public TimeComponent(Config config)
        {
            this.config = config;
            currentTick = Environment.TickCount64 / config.ServerTickMilliseconds;
        }


        /// <summary>Current tick</summary>
        public long TickCount => currentTick;

        /// <summary>Time elapsed since tick change</summary>
        public long TimeAfterTick => Environment.TickCount64 - (currentTick * config.ServerTickMilliseconds);

        /// <summary>Time to next tick</summary>
        public long TimeUntilTick => config.ServerTickMilliseconds - TimeAfterTick;



        public float DeltaTime => config.ServerTickMilliseconds / 1000f;


        public void NextTick() => currentTick++;

        
    }
}