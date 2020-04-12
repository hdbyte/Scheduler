using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HDByte.Scheduler
{
    public class Manager
    {
        public BlockingCollection<Job> _jobQueue = new BlockingCollection<Job>();

        public void Add(Action action)
        {
            var job = new Job() { Action = action, IsAsync = false };
            _jobQueue.Add(job);
        }

        public void AddAsync(Action action)
        {
            var job = new Job() { Action = action, IsAsync = true };
            _jobQueue.Add(job);
        }

        private void ActionQueueConsumer()
        {
            while (!_jobQueue.IsCompleted)
            {
                if (_jobQueue.TryTake(out Job job))
                {
                    if (job.IsAsync)
                        Task.Run(() => job.Action());
                    else
                        job.Action();
                }
            }
        }

        public void Start()
        {
            SetupThreadPool();

            var thread = new Thread(() => ActionQueueConsumer());
            thread.IsBackground = true;
            thread.Start();
        }

        public static void SetupThreadPool()
        {
            ThreadPool.GetMinThreads(out int minWorker, out int minIOC);
            ThreadPool.SetMinThreads(Configuration.MinimumThreadPool, minIOC);
        }
    }
}
