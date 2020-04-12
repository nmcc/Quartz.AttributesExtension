using System;

namespace Quartz
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JobAttribute : Attribute
    {
        public JobAttribute()
        {
        }

        public JobAttribute(string name, string group = null)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Group = group;
        }

        public string Name { get; }

        public string Group { get; }
    }
}
