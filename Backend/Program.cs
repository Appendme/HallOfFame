using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateBootstrapLogger();

			try
			{
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog((context, services, configuration) => configuration
					.ReadFrom.Configuration(context.Configuration)
					.ReadFrom.Services(services)
					.Enrich.FromLogContext()
					.WriteTo.Console())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
