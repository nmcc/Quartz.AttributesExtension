using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quartz.AttributesExtension.Reflection
{
    public class JobLocatorPerfTest
    {
        [Benchmark]
        public void GetAllJobsInAppDomain() => JobLocator.GetAllJobsInAppDomain();
    }
}
