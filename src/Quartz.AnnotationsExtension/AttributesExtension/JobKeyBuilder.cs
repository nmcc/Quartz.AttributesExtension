using System;

namespace Quartz.AttributesExtension
{
    internal static class JobKeyBuilder
    {
        public static JobKey BuildJobKey(JobAttribute jobAttribute, Type jobType)
        {
            if (string.IsNullOrWhiteSpace(jobAttribute.Name))
                return new JobKey(jobType.Name);
            else if (jobAttribute.Group == null)
                return new JobKey(jobAttribute.Name);

            return new JobKey(jobAttribute.Name, jobAttribute.Group);
        }

    }
}
