# Quartz Attributes Extension

This project enables the scheduling of Jobs with [Quartz.NET](https://www.quartz-scheduler.net/) using .NET Attributes.

It also enhances the configuration of `JobDataMap` and schedulers by using a convention on the application configuration file.

It currently supports the following:

* Registering an `IJob` class as a Quart job using Attributes in the code
* Setting properties on `JobDataMap` using Attributes in the code or by configuration 
* Scheduling Simple and Cron triggers using Attributes in the code

## Scheduling a job

To use the library, all you need to do is to add a reference to the `Quartz.AttributeExtensions` DLL to your project.

This will add a set of extenstion methods to `IScheduler` and make all the annotations available under the `Quartz` namespace.

To schedule a job:

```csharp
using Quartz;

class Program
{
    static void Main(string[] args)
    {
        var scheduler = StdSchedulerFactory.GetDefaultScheduler();

        // Extension method that configures job from the HelloJob class
        scheduler.ScheduleJob<HelloJob>();

        // and start it off
        scheduler.Start();
    }
}
```

You can also register all jobs by using reflection using the following extension method:

```csharp
using Quartz;

class Program
{
    static void Main(string[] args)
    {
        var scheduler = StdSchedulerFactory.GetDefaultScheduler();

        // Locates all classes that declare the [Job] attribute and schedules them
        scheduler.ScheduleAllJob();

        // and start it off
        scheduler.Start();
    }
}
```

## Attributes

* `[Job]` - registers the `Job` class on the scheduler
* `[CronTrigger]` - configures a Cron trigger using the specified arguments
* `[CronTriggerFromConfig]` - configures a Cron trigger using the configuration convention
* `[SimpleTrigger]` - configures a Simple trigger using the specified arguments
* `[SimpleTriggerFromConfig]` - configures a Simple trigger using the configuration convention
* `[JobData]` - Adds a hard-coded property to the `JobDataMap`
* `[JobDataElement]` - Adds property to the `JobDataMap` from the configuration convention

## Configuration conventions

Quartz.AttributesExtensions uses a convention to get data from the configuration file.

The convention is as follows:

* For Job Data Parameters
    * `Jobs.<JobKey>.<JobData_ParameterName>` - Sets Job Data parameters 
* For Cron Trigger: 
    * `Jobs.<JobKey>.<TriggerKey>.Cron` - Sets the Cron expression for a specific trigger
* For Simple Trigger: 
    * `Jobs.<JobKey>.<TriggerKey>.IntervalInSeconds` - Sets the trigger interval in ceconds for a specific trigger
    * `Jobs.<JobKey>.<TriggerKey>.RepeatForever` - Sets whether a specific trigger should be triggered forever (default: `false`)

The `<JobKey>` and `<TriggerKey>` are defined as `<Group>.<Name>`.

When the Job or Trigger are added to the `DEFAULT` group, the `<Group>` can be ommmited.

## Examples

### Job Data

```csharp
[Job(nameof(HelloJob))]
public class HelloJob : IJob
{
    [JobData("Hello")] // hard coded value
    public string Message { get; set; }

    [JobData] // gets value from Jobs.HelloJob.SomeNumber from configuration
    public int SomeNumber { get; set; }

    [JobData] // getsvalue from Jobs.HelloJob.SomeBoolean from configuration
    public bool SomeBoolean { get; set; }
}
```

### Cron Trigger with hard coded Cron expression

```csharp
[Job(nameof(HelloJob))]
[CronTrigger("0/10 * * * * ? *", name: "MyCronTrigger")]
public class HelloJob : IJob
{
    public void Execute(IJobExecutionContext context)
    {
    }
}
```

### Cron Trigger from the configuration file

`HelloJob.cs`

```csharp
[Job(nameof(HelloJob))]
[CronTriggerFromConfig(name: "MyCronTrigger")]
public class HelloJob : IJob
{
    public void Execute(IJobExecutionContext context)
    {
    }
}
```

`App.config`

```xml
<appSettings>
    <add key="Jobs.HelloJob.MyCronTrigger.Cron" value="0/10 * * * * ? *"/>
</appSettings>
```

### Simple Trigger with hard configuration repeating forever

```csharp
[Job(nameof(HelloJob))]
[SimpleTrigger(days: 0, hours: 0, minutes: 0, seconds: 10, name: "SimpleTrigger_Forever")]
public class HelloJob : IJob
{
    public void Execute(IJobExecutionContext context)
    {
    }
}
```

### Simple Trigger with hard configuration with limited repeat count

```csharp
[Job(nameof(HelloJob))]
[SimpleTrigger(days: 0, hours: 0, minutes: 0, seconds: 10, repeatCount: 50 name: "SimpleTrigger_Count")]
public class HelloJob : IJob
{
    public void Execute(IJobExecutionContext context)
    {
    }
}
```

### Simple Trigger from the configuration file repeateing forever

`HelloJob.cs`

```csharp
[Job(nameof(HelloJob))]
[SimpleTriggerFromConfig(name: "MySimpleTrigger")]
public class HelloJob : IJob
{
    public void Execute(IJobExecutionContext context)
    {
    }
}
```

`App.config`

```xml
<appSettings>
    <add key="Jobs.HelloJob.MySimpleTriggerConfig.IntervalInSeconds" value="5"/>
    <add key="Jobs.HelloJob.MySimpleTriggerConfig.RepeatForever" value="true"/>
</appSettings>
```

### Simple Trigger from the configuration file with limited repeat count

`HelloJob.cs`

```csharp
[Job(nameof(HelloJob))]
[SimpleTriggerFromConfig(name: "MySimpleTrigger")]
public class HelloJob : IJob
{
    public void Execute(IJobExecutionContext context)
    {
    }
}
```

`App.config`

```xml
<appSettings>
    <add key="Jobs.HelloJob.MySimpleTriggerConfig.IntervalInSeconds" value="5"/>
    <add key="Jobs.HelloJob.MySimpleTriggerConfig.RepeatCount" value="100"/>
</appSettings>
```