using JobRunnerService.Models;

namespace JobRunnerService.Interfaces
{
    public interface IJobService
    {
        Task<string> EnqueueJobAsync(string jobName);
        Task<Job> GetJobStatusAsync(string jobId);
        Task<IEnumerable<Job>> GetJobHistoryAsync();
    }
}