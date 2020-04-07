using BenchmarkDotNet.Running;
using Quartz.AttributesExtension.Reflection;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<JobLocatorPerfTest>();
        }
    }
}
