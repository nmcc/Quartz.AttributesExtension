using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("Quartz.AttributesExtension.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // To allow Moq to generate Mocks from internal interfaces
#endif