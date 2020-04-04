namespace Quartz.AttributesExtension.Trigger
{
    internal interface ITriggerBuilder
    {
        ITrigger BuildTrigger(ITriggerAttribute triggerAttribute, JobKey jobKey);
    }
}
