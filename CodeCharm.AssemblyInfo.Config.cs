using System.Reflection;
using System.Runtime.CompilerServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
#if DEV
[assembly: AssemblyConfiguration("DEV")]
[assembly: AssemblyInformationalVersion("DEV configuration: Local Development")]
#elif INTG
[assembly: AssemblyConfiguration("INTG")]
[assembly: AssemblyInformationalVersion("INTG configuration: Integration")]
#elif QA
[assembly: AssemblyConfiguration("QA")]
[assembly: AssemblyInformationalVersion("QA configuration: Quality Assurance")]
#elif UAT
[assembly: AssemblyConfiguration("UAT")]
[assembly: AssemblyInformationalVersion("UAT configuration: User Acceptance Test")]
#elif TEST
[assembly: AssemblyConfiguration("TEST")]
[assembly: AssemblyInformationalVersion("TEST configuration: Load/Stress/Security Test")]
#elif STAGCORE
[assembly: AssemblyConfiguration("STAGCORE")]
[assembly: AssemblyInformationalVersion("STAGCORE configuration: Staging for Intranet")]
#elif STAG
[assembly: AssemblyConfiguration("STAG")]
[assembly: AssemblyInformationalVersion("STAG configuration: Staging for DMZ")]
#elif PRODCORE
[assembly: AssemblyConfiguration("PRODCORE")]
[assembly: AssemblyInformationalVersion("PRODCORE configuration: Production for Intranet")]
#elif PROD
[assembly: AssemblyConfiguration("PROD")]
[assembly: AssemblyInformationalVersion("PROD configuration: Production for DMZ")]
#else
[assembly: AssemblyConfiguration("NOTSET")]
[assembly: AssemblyInformationalVersion("Configuration not set")]
#endif
[assembly: AssemblyCompany("Code Charm")]
[assembly: AssemblyCopyright("Copyright © 2015 by Alan McBee")]
