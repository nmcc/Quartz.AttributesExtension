namespace Quartz.AttributesExtension.JobData
{
    internal interface IJobDataBuilder
    {
        JobDataMap Build<JobType>(JobKey jobKey) where JobType : IJob;
    }
}