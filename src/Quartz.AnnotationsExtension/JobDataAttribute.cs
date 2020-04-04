using System;

namespace Quartz
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class JobDataAttribute : Attribute
    {
        public JobDataAttribute()
        {
        }

        public JobDataAttribute(object value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public object Value { get;  }
    }
}
