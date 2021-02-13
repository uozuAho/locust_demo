using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace apiserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly JobQueue _jobQueue;
        private readonly ILogger<JobsController> _logger;
        private readonly JobTracker _jobTracker;

        public JobsController(
            JobQueue jobQueue,
            JobTracker jobTracker,
            ILogger<JobsController> logger)
        {
            _jobQueue = jobQueue;
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
            var jobId = _jobQueue.Add(message);

            _logger.LogInformation($"added job {jobId} to queue");

            return jobId;
        }
    }
}
