namespace Quartz.AttributesExtension
{
    [Job]
    internal class InterruptableJob : IInterruptableJob
    {
        [JobData]
        public string Message { get; set; }

        public void Execute(IJobExecutionContext context)
        {
            // Do nothing
        }

        public void Interrupt()
        {
            // Do nothing
        }
    }
}
