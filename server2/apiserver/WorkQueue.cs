using System;
using System.Collections.Concurrent;

namespace apiserver
{
    public class WorkQueue
    {
        private readonly JobTracker _jobTracker;
        private readonly ConcurrentQueue<Job> _queue = new();
        private int _jobCounter = 0;

        public WorkQueue(JobTracker jobTracker)
        {
            _jobTracker = jobTracker;
        }

        public int Add(string message)
        {
            var job = CreateJob(message);

            _jobTracker.Add(job);
            _queue.Enqueue(job);

            return job.Id;
        }

        private Job CreateJob(string message)
        {
            return new Job
            {
                Id = ++_jobCounter,
                Message = message,
                Status = "new",
                Created = DateTime.Now
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