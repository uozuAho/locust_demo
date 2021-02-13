using System.Collections.Generic;

namespace apiserver
{
    public class JobTracker
    {
        private readonly Dictionary<int, Job> _jobs = new();

        public Job FindJob(int id)
        {
            return _jobs[id];
        }

        public void Add(Job job)
        {
            _jobs[job.Id] = job;
        }
    }
}