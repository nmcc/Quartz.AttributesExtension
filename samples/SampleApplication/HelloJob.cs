using Quartz;
using System;

namespace SampleApplication
{
    [Job(nameof(HelloJob))]
    [CronTrigger("0/10 * * * * ? *", name: "CronTrigger")]
    [CronTriggerFromConfig()]
    [SimpleTrigger(days: 0, hours: 0, minutes: 0, seconds: 10, name: "SimpleTrigger")]
    [SimpleTriggerFromConfig("SimpleTriggerConfig")]
    public class HelloJob : IJob
    {
        [JobData("Hello")]
        public string Message { get; set; }

        [JobData]
        public int Number { get; set; }

        [JobData]
        public bool Boolean { get; set; }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{context.Trigger.Key} -> {DateTime.Now.ToLongTimeString()}: {this.Message ?? "it's null"} - number {this.Number} - Boolean {this.Boolean}");
        }
    }
}
