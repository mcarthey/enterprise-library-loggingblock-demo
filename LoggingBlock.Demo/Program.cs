using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace LoggingBlock.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Option to programatically create logging configuration
            // Create trace listeners
            //            FlatFileTraceListener flatFileTraceListener = new FlatFileTraceListener("logging.txt");

            // Build Configuration
            //            LoggingConfiguration config = new LoggingConfiguration();
            //            config.AddLogSource("General", SourceLevels.All, true);

            // Configure the LogWriter instance
            //            LogWriter defaultWriter = new LogWriter(config);

            // Use log write factory to load from app.config
            LogWriterFactory logWriterFactory = new LogWriterFactory();

            // Create the LogWrite instance
            LogWriter logWriter = logWriterFactory.Create();

            // Create debug provider
            var extendedInfo = new Dictionary<string, object>();
            var debug = new DebugInformationProvider();
            debug.PopulateDictionary(extendedInfo);

            // Create the Log Entry
            var logEntry = new LogEntry
            {
                Message = "This is an error message",
                Categories = new List<string> { "Debug" },
                ExtendedProperties = extendedInfo
            };

            // Prepare the trace manager
            TraceManager traceManager = new TraceManager(logWriter);

            // Log
            // checking app.config "tracingEnabled" - <loggingConfiguration name="" tracingEnabled="true">
            if (logWriter.IsLoggingEnabled())
            {
                logWriter.Write("This is a Demo log message", "Demo");
                logWriter.Write("This is an application error", "Error");
                logWriter.Write("This is a general log message");

                using (traceManager.StartTrace("Debug"))
                {
                    logWriter.Write(logEntry);
                }
            }
        }
    }
}
