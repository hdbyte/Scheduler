using System;
using System.Collections.Generic;
using System.Text;

namespace HDByte.Scheduler
{
    public static class Configuration
    {
        private static int _minimimThreadPool;
        public static int MinimumThreadPool
        {
            get =>_minimimThreadPool;
            set
            {
                _minimimThreadPool = value;
                Manager.SetupThreadPool();
            }
        }
    }
}
