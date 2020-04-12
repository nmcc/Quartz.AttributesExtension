using System;
using static Quartz.QuartzConstants;

namespace Quartz
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class CronTriggerFromConfigAttribute : Attribute, ITriggerAttribute
    {
        public CronTriggerFromConfigAttribute()
        {
        }

        public CronTriggerFromConfigAttribute(string name, string group = DefaultGroup)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Group = group ?? throw new ArgumentNullException(nameof(group));
        }

        public string Name { get; }

        public string Group { get; }
    }
}
