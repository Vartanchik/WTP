using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;

namespace WTP.Logging
{
    public class SerilogLog : ILog
    {
        public SerilogLog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .WriteTo.File(path: @"../WTP.Logging/logs/log.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                .WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces)
                .CreateLogger();
        }

        public void Verbose(string message)
        {
            Log.Verbose(message);
        }

        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Information(string message)
        {
            Log.Information(message);
        }

        public void Warning(string message)
        {
            Log.Warning(message);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Fatal(string message)
        {
            Log.Fatal(message);
        }
    }
}
