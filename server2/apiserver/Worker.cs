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
                if (_queue.HasWork())
                {
                    var job = _queue.Pop();
                    job.Started = DateTime.Now;
                    Thread.Sleep(1000);
                    job.Completed = DateTime.Now;
                    job.Status = "done";
                    PrintCompletedJob(job);
                }
            }
        }

        private void PrintCompletedJob(Job job)
        {
            Console.WriteLine($"Job {job.Id} complete");
            Console.WriteLine($"    Time waiting in queue: {job.Started - job.Created}");
            Console.WriteLine($"    Time processing      : {job.Completed - job.Started}");
            Console.WriteLine($"    Total                : {job.Completed - job.Created}");
        }
    }
}