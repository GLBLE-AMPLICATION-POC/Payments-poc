using payments-service.Data;
using JobRunnerService.Interfaces;
using JobRunnerService.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace JobRunnerService.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly RedisContext _redisContext;
        private readonly IDatabase _database;

        public JobRepository(RedisContext redisContext)
        {
            _redisContext = redisContext;
            _database = _redisContext.Database;
        }

        /// <summary>
        /// Creates job in the database
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task<string> CreateJobAsync(Job job)
        {
            job.Id = Guid.NewGuid().ToString();
            job.CreatedTime = DateTime.UtcNow;
            job.Status = JobStatus.Pending;

            var key = $"job:{job.Id}";
            await _database.StringSetAsync(key, JsonConvert.SerializeObject(job));
            return job.Id;
        }

        /// <summary>
        /// Get specific job from the database
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<Job> GetJobAsync(string jobId)
        {
            var key = $"job:{jobId}";
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Job>(value) ?? new Job();
        }

        /// <summary>
        /// Update job in the database
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task UpdateJobAsync(Job job)
        {
            var key = $"job:{job.Id}";
            await _database.StringSetAsync(key, JsonConvert.SerializeObject(job));
        }

        /// <summary>
        /// Get all jobs from the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Job>> GetJobHistoryAsync()
        {
            // This is a simple implementation. 
            // For production, consider using Redis Sorted Sets for efficient history retrieval.
            var keys = _redisContext.Database.Multiplexer.GetServer(_redisContext.Database.Multiplexer.GetEndPoints()[0]).Keys(pattern: "job:*");
            var jobs = new List<Job>();
            foreach (var key in keys)
            {
                var value = await _database.StringGetAsync(key);
                if (!value.IsNullOrEmpty)
                {
                    jobs.Add(JsonConvert.DeserializeObject<Job>(value));
                }
            }
            return jobs.OrderByDescending(j => j.CreatedTime);
        }
    }
}