namespace Quartz
{
    internal interface ITriggerAttribute
    {
        string Name { get; }

        string Group { get; }
    }
}
