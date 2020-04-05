using System;

namespace Quartz.AttributesExtension.Trigger
{
    internal static class TriggerKeyBuilder
    {
        public static TriggerKey BuildTriggerKey(ITriggerAttribute triggerAttribute)
        {
            if (string.IsNullOrEmpty(triggerAttribute.Name))
            {
                return new TriggerKey(Guid.NewGuid().ToString());
            }
            else if (string.IsNullOrEmpty(triggerAttribute.Group))
            {
                return new TriggerKey(triggerAttribute.Name);
            }
            else
            {
                return new TriggerKey(triggerAttribute.Name, triggerAttribute.Group);
            }
        }
    }
}
