using System;
using System.Collections.Generic;
using System.Text;

namespace HDByte.Scheduler
{
    public class Job
    {
        public Action Action { get; set; }

        public string Name { get; set; }
    }
}
