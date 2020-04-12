using System;
using static Quartz.QuartzConstants;

namespace Quartz
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class SimpleTriggerAttribute : Attribute, ITriggerAttribute
    {
        public SimpleTriggerAttribute(int days, int hours, int minutes, int seconds, int repeatCount)
        {
            this.Interval = new TimeSpan(days, hours, minutes, seconds);
            this.RepeatForever = false;
            this.RepeatCount = repeatCount;
        }

        public SimpleTriggerAttribute(int days, int hours, int minutes, int seconds)
        {
            this.Interval = new TimeSpan(days, hours, minutes, seconds);
            this.RepeatForever = true;
        }

        public SimpleTriggerAttribute(int days, int hours, int minutes, int seconds, string name, string group = DefaultGroup)
            : this(days, hours, minutes, seconds)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Group = group ?? throw new ArgumentNullException(nameof(group));
        }

        public SimpleTriggerAttribute(int days, int hours, int minutes, int seconds, int repeatCount, string name, string group = DefaultGroup)
            : this(days, hours, minutes, seconds, repeatCount)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Group = group ?? throw new ArgumentNullException(nameof(group));
        }

        public string Name { get; }

        public string Group { get; }

        public TimeSpan Interval { get; }

        public bool RepeatForever { get; }

        public int RepeatCount { get; }
    }
}
