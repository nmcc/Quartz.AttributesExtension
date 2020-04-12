using Quartz.AttributesExtension.Configuration;
using System;
using System.Linq;

namespace Quartz.AttributesExtension.JobData
{
    internal class JobDataBuilder : IJobDataBuilder
    {
        private readonly IConfigurationProvider configurationProvider;

        public JobDataBuilder()
        {
            this.configurationProvider = new ConfigurationProvider();
        }

        public JobDataBuilder(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public JobDataMap Build(Type jobType, JobKey jobKey)
        {
            var jobDataMap = new JobDataMap();

            foreach (var property in jobType.GetProperties())
            {
                var jobDataAttribute = property.GetCustomAttributes(typeof(JobDataAttribute), inherit: false)
                    .Cast<JobDataAttribute>()
                    .FirstOrDefault();

                if (jobDataAttribute == null)
                {
                    continue;
                }
                
                if (jobDataAttribute.Value == null)
                {
                    var key = ConfigurationKeyBuilder.Build(jobKey, property.Name);
                    var value = this.configurationProvider.GetString(key);

                    jobDataMap.Add(property.Name, value);
                }
                else
                {
                    jobDataMap.Add(property.Name, jobDataAttribute.Value);
                }
            }

            return jobDataMap;
        }
    }
}
