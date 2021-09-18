using Npgsql.Logging;
using System;

namespace Mod.Utility.Npgsql.Logging
{
    public class NpgsqlLoggingProviderWithILogger : INpgsqlLoggingProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public NpgsqlLoggingProviderWithILogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public NpgsqlLogger CreateLogger(string name)
        {
            return new NpgsqlLoggerWithILogger(_serviceProvider, name);
        }
    }

}
