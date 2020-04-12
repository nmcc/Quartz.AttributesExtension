using System;
using System.Collections.Generic;
using System.Linq;

namespace Quartz.AttributesExtension.Reflection
{
    internal static class JobLocator
    {
        public static IEnumerable<Type> GetAllJobsInAppDomain()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.GlobalAssemblyCache
                        && !assembly.IsDynamic
                        && assembly.Location.StartsWith(AppDomain.CurrentDomain.BaseDirectory))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsDefined(typeof(JobAttribute), inherit: false));
        }
    }
}
