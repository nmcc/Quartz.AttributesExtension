namespace Quartz.AttributesExtension.Trigger
{
    internal class TriggerBuilderFactory : ITriggerBuilderFactory
    {
        public ITriggerBuilder GetTriggerBuilder(ITriggerAttribute triggerAttr)
        {
            if (triggerAttr is SimpleTriggerAttribute)
            {
                return new SimpleTriggerAttributeBuilder();
            }
            else if (triggerAttr is SimpleTriggerFromConfigAttribute)
            {
                return new SimpleTriggerFromConfigAttributeBuilder();
            }
            else if (triggerAttr is CronTriggerAttribute)
            {
                return new CronTriggerAttributeBuilder();
            }
            else if (triggerAttr is CronTriggerFromConfigAttribute)
            {
                return new CronTriggerFromConfigAttributeBuilder();
            }

            return null;
        }
    }
}
