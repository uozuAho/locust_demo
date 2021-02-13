using System.Collections.Concurrent;

namespace apiserver
{
    public class WorkQueue
    {
        private readonly ConcurrentQueue<Job> _queue = new();
        private int _jobCounter = 0;

        public int Add(string message)
        {
            var job = CreateJob(message);

            _queue.Enqueue(job);

            return job.Id;
        }

        private Job CreateJob(string message)
        {
            return new Job
            {
                Id = ++_jobCounter,
                Message = message
            };
        }

        public bool HasWork()
        {
            return !_queue.IsEmpty;
        }

        public Job Pop()
        {
            _queue.TryDequeue(out var job);
            return job;
        }
    }
}