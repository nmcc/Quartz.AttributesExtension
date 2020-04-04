namespace Quartz.AttributesExtension
{
    [Job(nameof(SampleJob))]
    [SimpleTriggerFromConfig("trigger1")]
    internal class SampleJob : IJob
    {
        [JobData("Lorem")]
        public string Param1 { get; set; }

        [JobData]
        public bool BoolParam { get; set; }

        [JobData]
        public int IntParam { get; set; }

        public void Execute(IJobExecutionContext context)
        {
        }
    }
}
