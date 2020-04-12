namespace Quartz.AttributesExtension.Trigger
{
    internal interface ITriggerBuilderFactory
    {
        ITriggerBuilder GetTriggerBuilder(ITriggerAttribute triggerAttr);
    }
}