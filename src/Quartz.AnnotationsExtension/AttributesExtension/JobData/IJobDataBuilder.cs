using System;

namespace Quartz.AttributesExtension.JobData
{
    internal interface IJobDataBuilder
    {
        JobDataMap Build(Type jobType, JobKey jobKey);
    }
}