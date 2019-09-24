using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ExampleService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Settings = new ConfigurationBuilder()
				.SetBasePath(Directory)
				.AddJsonFile("Settings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			StartWebHost();
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
