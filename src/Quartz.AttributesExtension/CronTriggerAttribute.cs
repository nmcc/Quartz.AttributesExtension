using System;
using static Quartz.QuartzConstants;

namespace Quartz
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class CronTriggerAttribute : Attribute, ITriggerAttribute
    {
        public CronTriggerAttribute(string expression)
        {
            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public CronTriggerAttribute(string expression, string name, string group = DefaultGroup) : this(expression)
        {
            this.Name = name ?? throw new ArgumentNullException(name);
            this.Group = group ?? throw new ArgumentNullException(group);
        }

        public string Expression { get; }

        public string Name { get; }

        public string Group { get; }
    }
}
