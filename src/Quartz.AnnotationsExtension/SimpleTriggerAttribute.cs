using System;
using static Quartz.QuartzConstants;

namespace Quartz
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class SimpleTriggerAttribute : Attribute, ITriggerAttribute
    {

        public SimpleTriggerAttribute(int days, int hours, int minutes, int seconds, bool repeatForever)
        {
            this.Interval = new TimeSpan(days, hours, minutes, seconds);
            this.RepeatForever = repeatForever;
        }

        public SimpleTriggerAttribute(int days, int hours, int minutes, int seconds, bool repeatForever, string name, string group = DefaultGroup)
            : this(days, hours, minutes, seconds, repeatForever)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Group = group ?? throw new ArgumentNullException(nameof(group));
        }

        public string Name { get; }

        public string Group { get; }

        public TimeSpan Interval { get; }

        public bool RepeatForever { get; }
    }
}
