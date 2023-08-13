using BackUpMe.Services;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

namespace BackUpMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args)
             .UseWindowsService(options =>
             {
                 options.ServiceName = "Back Me Up";
             })
             .ConfigureServices((context, services) =>
             {
#pragma warning disable CA1416 // Validate platform compatibility
                 LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(services);
#pragma warning restore CA1416 // Validate platform compatibility

                 services.AddSingleton<FTPService>();
                 services.AddHostedService<WindowsBackgroundService>();

                 // See: https://github.com/dotnet/runtime/issues/47303
                 services.AddLogging(builder =>
                 {
                     builder.AddConfiguration(
                         context.Configuration.GetSection("Logging"));
                 });
             });






            var host = builder.Build();
            host.Run();
        }
    }
}