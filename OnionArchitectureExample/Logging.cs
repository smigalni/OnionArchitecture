using System;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RollingFile;


namespace OnionArchitectureExample
{   
    public  class Logging
    {
        public  void WriteToLog(StatusEnum serviceStatus, string serviceName, int interval)
        {
            //create new serilog configuration to get Source correct for service we are checking
            var log = Create(serviceName);
            var absolutvalue = Math.Abs(interval);
            switch (serviceStatus)
            {
                case StatusEnum.Bad:
                    log.Error($"{serviceName} has status {serviceStatus}. There was no updates last {absolutvalue} minute(s)!");
                    break;
                case StatusEnum.Good:
                    log.Information($"{serviceName} has status {serviceStatus}. Services works properly!");
                    break;
                default:
                    log.Fatal("Unexpected error. There is unknown service status!");
                    break;
            }
        }
        public  ILogger Create(string serviceName)
        {
            return Log.Logger = new LoggerConfiguration()
                 .ReadFrom.AppSettings()
                 .WriteTo.Sink(
                     new RollingFileSink(@"C:\DRIV-Logs\Log-{Date}-" + serviceName.Replace(".", "-") + ".txt",
                         new JsonFormatter(), null, 5), LogEventLevel.Information)
                 .WriteTo.ColoredConsole()
                 .WriteTo.Trace()
                 //.Destructure.UsingAttributes()
                 .Enrich.WithMachineName()
                 .Enrich.WithProperty("Source", serviceName)
                 .Enrich.WithThreadId()
                 .Enrich.FromLogContext()
                 .WriteTo.EventLog(serviceName)
                 .CreateLogger();
        }
    }
}