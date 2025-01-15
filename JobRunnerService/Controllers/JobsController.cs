using JobRunnerService.Interfaces;
using JobRunnerService.Models;
using JobRunnerService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobRunnerService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IJobService jobService, ILogger<JobsController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateJob([FromBody] string jobName)
        {
            if (string.IsNullOrEmpty(jobName))
            {
                return BadRequest("Job name cannot be empty.");
            }

            _logger.LogInformation($"Enqueuing job: {jobName}");
            var jobId = await _jobService.EnqueueJobAsync(jobName);
            return CreatedAtAction(nameof(CreateJob), new { jobId = jobId }, new { job = jobId, status = "Enqueued" });
        }

        [HttpGet("{jobId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Job>> GetJobStatus(string jobId)
        {
            _logger.LogInformation($"Getting status for job: {jobId}");
            var job = await _jobService.GetJobStatusAsync(jobId);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }
    }
}