using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;

namespace ExampleService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			SetupLogging();

			Settings = new ConfigurationBuilder()
				.SetBasePath(Directory)
				.AddJsonFile("Settings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			StartWebHost();
		}

		private static void SetupLogging()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.RollingFile("app-{Date}.log")
				.CreateLogger();
		}

		private static void StartWebHost()
		{
			try
			{
				WebHost = new WebHostBuilder()
					.UseKestrel()
					.UseConfiguration(Settings)
					.UseStartup<ProgramSetup>()
					.UseUrls($"http://*:{Settings.GetValue<int>("WebPort")}")
					.UseSerilog()
					.Build();

				WebHost.Run();
			}
			catch (Exception exception)
			{
				Console.Error.WriteLine(exception.Message);
				Console.Error.WriteLine(exception.StackTrace);
			}
		}

		public static string Directory { get; } = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

		public static IConfigurationRoot Settings { get; private set; }
		public static IWebHost WebHost { get; private set; }
	}
}
