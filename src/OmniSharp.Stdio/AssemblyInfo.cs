using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OmniSharp")]
[assembly: InternalsVisibleTo("TestUtility")]
[assembly: InternalsVisibleTo("OmniSharp.Stdio.Tests")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]