using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HDByte.Scheduler
{
    public class Manager
    {
        public BlockingCollection<Job> _jobQueue = new BlockingCollection<Job>();

        public void Add(Action jobAction)
        {
            AddToJobQueue(jobAction, "");
        }

        public void Add(string jobName, Action jobAction)
        {
            AddToJobQueue(jobAction, jobName);
        }

        private void AddToJobQueue(Action jobAction, string jobName = "")
        {
            var job = new Job() { Action = jobAction, Name = jobName };
            _jobQueue.Add(job);
        }

        public void AddAsync(Action action)
        {
            Task.Run(() => action());
        }

        private void JobQueueThreadConsumer()
        {
            while (!_jobQueue.IsCompleted)
            {
                if (_jobQueue.TryTake(out Job job))
                {
                    job.Action();
                }
            }
        }

        public void Start()
        {
            SetupThreadPool();

            var thread = new Thread(() => JobQueueThreadConsumer());
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
