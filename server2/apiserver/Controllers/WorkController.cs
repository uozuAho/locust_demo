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

        public WorkController(
            WorkQueue workQueue,
            ILogger<WorkController> logger)
        {
            _workQueue = workQueue;
            _logger = logger;
        }

        [HttpGet]
        public int Get()
        {
            return 1;
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
