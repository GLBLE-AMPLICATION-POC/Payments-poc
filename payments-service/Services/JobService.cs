using payments-service.Interfaces;
using JobRunnerService.Models;
using JobRunnerService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobRunnerService.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private static readonly List<string> _runningJobs = new List<string>();

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<string> EnqueueJobAsync(string jobName)
        {
            var job = new Job { Name = jobName };
            var jobId = await _jobRepository.CreateJobAsync(job);

            // Simulate job execution (replace with actual job logic)
            _ = Task.Run(() => RunJobAsync(jobId)); // Fire and forget

            return jobId;
        }

        public async Task<Job> GetJobStatusAsync(string jobId)
        {
            return await _jobRepository.GetJobAsync(jobId);
        }

        public async Task<IEnumerable<Job>> GetJobHistoryAsync()
        {
            return await _jobRepository.GetJobHistoryAsync();
        }

        private async Task RunJobAsync(string jobId)
        {
            if (_runningJobs.Contains(jobId))
            {
                return;
            }

            _runningJobs.Add(jobId);

            try
            {
                var job = await _jobRepository.GetJobAsync(jobId);
                job.Status = JobStatus.Running;
                job.StartTime = DateTime.UtcNow;
                await _jobRepository.UpdateJobAsync(job);

                // Simulate work
                await Task.Delay(10000);

                // Update job completion
                job.Status = JobStatus.Completed;
                job.EndTime = DateTime.UtcNow;
                job.Result = "Job completed successfully.";
                await _jobRepository.UpdateJobAsync(job);
            }
            catch (Exception ex)
            {
                var job = await _jobRepository.GetJobAsync(jobId);
                job.Status = JobStatus.Failed;
                job.EndTime = DateTime.UtcNow;
                job.Result = $"Job failed: {ex.Message}";
                await _jobRepository.UpdateJobAsync(job);
            }
            finally
            {
                _runningJobs.Remove(jobId);
            }
        }
    }
}