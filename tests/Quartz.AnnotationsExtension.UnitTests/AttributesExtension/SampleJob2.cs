namespace Quartz.AttributesExtension
{
    [Job(nameof(SampleJob2))]
    public sealed class SampleJob2 : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
        }
    }
}
