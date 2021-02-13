using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace apiserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkController : ControllerBase
    {
        private readonly WorkQueue _workQueue;
        private readonly ILogger<WorkController> _logger;
        private readonly JobTracker _jobTracker;

        public WorkController(
            WorkQueue workQueue,
            JobTracker jobTracker,
            ILogger<WorkController> logger)
        {
            _workQueue = workQueue;
            _logger = logger;
            _jobTracker = jobTracker;
        }

        [HttpGet]
        [Route("{id}")]
        public string GetStatus(int id)
        {
            return _jobTracker.FindJob(id).Status;
        }

        [HttpPost]
        public int Post(string message)
        {
            var jobId = _workQueue.Add(message);

            _logger.LogInformation($"added job {jobId} to queue");

            return jobId;
        }
    }
}
