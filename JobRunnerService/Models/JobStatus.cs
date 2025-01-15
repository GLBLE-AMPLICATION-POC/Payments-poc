namespace JobRunnerService.Models
{
    public enum JobStatus
    {
        Pending = 0,
        Running = 1,
        Completed = 2,
        Failed = 3,
        Skipping = 4
    }
}