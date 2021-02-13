using System;
using System.Threading;
using System.Threading.Tasks;

namespace apiserver
{
    public class Worker
    {
        private readonly WorkQueue _queue;

        public Worker(WorkQueue queue)
        {
            _queue = queue;
        }

        public void Start()
        {
            Task.Run(RunWorkLoop);
        }

        private void RunWorkLoop()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (_queue.HasWork())
                {
                    var job = _queue.Pop();
                    job.Status = "done";
                    Console.WriteLine($"Completed job {job.Id}");
                }
            }
        }
    }
}