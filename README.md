# INpgsqlLoggingProvider implementation with Microsoft.Extensions.Logging.ILogger

This repository contains custom [INpgsqlLoggingProvider](https://www.npgsql.org/doc/logging.html) implementation that utilizes Microsoft.Extensions.Logging.ILogger underneath. Npgsql log messages are written to all log providers added, obeying standart Microsoft.Extensions.Logging configuration rules.

## Setup

To hook up custom INpgsqlLoggingProvider log provider, inject NpgsqlLoggingProviderWithILogger class while building host.

```csharp
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddSingleton<INpgsqlLoggingProvider, NpgsqlLoggingProviderWithILogger>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });
```

Then enable Npgsql logging on application level, set Npgsql static parameters at the beginning of application, before invoking any other Npgsql method.

```csharp
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            //Configure PostgreSQL static parameters
            NpgsqlLogManager.Provider = host.Services.GetRequiredService<INpgsqlLoggingProvider>();
            //Enable parameter value logging. These may contain sensitive information.
            NpgsqlLogManager.IsParameterLoggingEnabled = true;

            host.Run();
        }
```

## Configuration

The configuration is setup in the Logging section of the configuraton json file.
Use `Npgsql` category under `Logging` block to configure log level for all Npgsql logs.
For information on LogLevel configuration, check [Logging in .NET Core and ASP.NET Core documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0).

```json
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "Npgsql": "Debug"
    }
  }
```
