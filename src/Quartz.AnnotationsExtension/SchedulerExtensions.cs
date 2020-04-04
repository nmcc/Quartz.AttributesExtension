using Quartz.AttributesExtension;

namespace Quartz
{
    public static class SchedulerExtensions
    {
        public static void ScheduleJob<JobType>(this IScheduler scheduler) where JobType : IJob
        {
            new JobScheduler(scheduler).ScheduleJob<JobType>();
        }
    }
}
