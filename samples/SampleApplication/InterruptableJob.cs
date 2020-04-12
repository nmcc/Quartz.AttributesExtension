using Quartz;
using System;

namespace SampleApplication
{
    [Job]
    [SimpleTrigger(0, 0, 0, 5)]
    public class InterruptableJob : IInterruptableJob
    {
        [JobData]
        public string Message { get; set; }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Interruptable job was triggered: {Message}");
        }

        public void Interrupt()
        {
            Console.WriteLine("Interruptable job has been interrupted");
        }
    }
}
