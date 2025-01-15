using JobRunnerService.Models;

namespace JobRunnerService.Interfaces
{
    public interface IJobRepository
    {
        Task<string> CreateJobAsync(Job job);
        Task<Job> GetJobAsync(string jobId);
        Task UpdateJobAsync(Job job);
        Task<IEnumerable<Job>> GetJobHistoryAsync();
    }
}