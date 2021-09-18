using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;
using System;

namespace Mod.Utility.Npgsql.Logging
{
    public class NpgsqlLoggerWithILogger : NpgsqlLogger
    {
        private readonly ILogger _logger;

        public NpgsqlLoggerWithILogger(IServiceProvider serviceProvider, string name)
        {
            _logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(name);
        }

        public override bool IsEnabled(NpgsqlLogLevel level)
        {
            return _logger.IsEnabled(ConvertLogLovel(level));
        }

        public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception exception = null)
        {
            _logger.Log(ConvertLogLovel(level), exception, "{connectorId}: {msg}", connectorId, msg);
        }

        private LogLevel ConvertLogLovel(NpgsqlLogLevel level)
        {
            return level switch
            {
                NpgsqlLogLevel.Debug => LogLevel.Debug,
                NpgsqlLogLevel.Error => LogLevel.Error,
                NpgsqlLogLevel.Fatal => LogLevel.Critical,
                NpgsqlLogLevel.Info => LogLevel.Information,
                NpgsqlLogLevel.Trace => LogLevel.Trace,
                NpgsqlLogLevel.Warn => LogLevel.Warning,
                _ => LogLevel.None
            };
        }
    }

}
