using System;

namespace JobRunnerService.Models
{
    public class Job
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public DateTime CreatedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public JobStatus Status { get; set; }
        public string Result { get; set; } = default!; // Store any result/output
    }
}