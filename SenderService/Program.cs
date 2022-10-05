using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NTB.SenderService.Data;

//dotnet publish -r win-x64 -c Release
//sc create NTBSendService BinPath=[path].exe 9.9MB
//sc start NTBSendService
//sc stop NTBSendService
//sc delete NTBSendService

namespace NTB.SenderService
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			using IHost host = CreateHostBuilder(args).Build();
			await host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((hostContext, config) =>
			{
				var env = hostContext.HostingEnvironment;

				config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
			})
			.ConfigureServices((hostContext, services) =>
			{
				var env = hostContext.HostingEnvironment;

				// Настройка БД.
				services.AddDbContext<DatabaseContext>(options =>
				{
					options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"),
						// Разделение join на отдельные запросы
						o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
						);

					if (env.IsDevelopment() || env.IsStaging())
					{
						options.LogTo(message => Debug.WriteLine(message));
						options.EnableSensitiveDataLogging();
						options.EnableDetailedErrors();
					}
				});

				// register DI
				//services.AddScoped<ISender, FileSender>();
				services.AddScoped<ISender, TelegramSender>();
				services.AddScoped<BroadcastService>();

				// конфигурация
				services.Configure<SenderServiceSettings>(hostContext.Configuration.GetSection(SenderServiceSettings.SectionName));
				services.Configure<FileSenderSettings>(hostContext.Configuration.GetSection("FileSenderSettings"));
				services.Configure<TelegramSenderSettings>(hostContext.Configuration.GetSection("TelegramSenderSettings"));

				services.AddHostedService<SenderWorker>();
			})
			//.UseDefaultServiceProvider((context, options) =>{ options.ValidateOnBuild = true; } )
			.UseWindowsService(options =>
			{
				options.ServiceName = "NTBSendService";
			})
			.ConfigureLogging((hostContext, logging) =>
			{
				// See: https://github.com/dotnet/runtime/issues/47303
				logging.AddConfiguration(
					hostContext.Configuration.GetSection("Logging"));
				logging.AddEventLog();
			});
	}
}
