using FileRegistrationApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace FileRegistrationApp;

internal class Program
{
	static async Task Main(string[] args)
	{
		ConfigureLogging();
		try
		{
			Log.Information("Starting application...");

			var builder = new HostBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					services.AddSingleton<FileWatherServiceConfig>(GetWatcherConfig(args));
					services.AddHostedService<FileWatcherService>();
				})
				.UseSerilog()
				.UseConsoleLifetime();

			await builder.Build().RunAsync();
		}
		catch (Exception ex)
		{
			Log.Fatal(ex, "Application terminated unexpectedly");
		}
		finally
		{
			Log.CloseAndFlush();
		}
	}

	private static void ConfigureLogging()
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Information()
			.Enrich.FromLogContext()
			.WriteTo.Console(
				 outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
			.WriteTo.File(
				path: "logs/app-.log",
				rollingInterval: RollingInterval.Day,
				retainedFileCountLimit: 7,
				outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
				shared: true)
			.CreateLogger();
	}

	private static FileWatherServiceConfig GetWatcherConfig(string[] args)
	{
		var config = new FileWatherServiceConfig();
		if (args.Length == 0)
		{
			config.Path = ConfigurationManager.AppSettings["RegisterDirectoryPath"];
		}
		else
		{
			config.Path = args[0];
		}
		return config;
	}
}
