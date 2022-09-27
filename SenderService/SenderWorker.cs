using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NTB.SenderService
{
	/// <summary>
	/// Конфигурация сервиса отправки уведомлений
	/// </summary>
	public class SenderServiceSettings
	{
		public static string SectionName => "SenderServiceSettings";

		/// <summary>
		/// Интервал запуска, секунды
		/// </summary>
		public int Interval { get; set; }
	}

	public class SenderWorker : IHostedService, IDisposable
	{
		protected readonly ILogger<SenderWorker> _logger;

		protected readonly IServiceScopeFactory _serviceScopeFactory;

		/// <summary>
		/// Таймер запуска рассылки
		/// </summary>
		private Timer _sendTimer;

		/// <summary>
		/// Интервал запуска рассылки
		/// </summary>
		public TimeSpan Interval { get; set; }

		public SenderWorker(IOptions<SenderServiceSettings> config, IServiceScopeFactory serviceScopeFactory, ILogger<SenderWorker> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

			int interval = config?.Value?.Interval ?? 60;
			Interval = TimeSpan.FromSeconds(interval > 10 ? interval : 60);

		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_sendTimer?.Dispose();
		}

		/// <inheritdoc cref="IHostedService"/>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Starting sender");
			_sendTimer = new Timer(TimerTask, null, TimeSpan.FromSeconds(10), Interval);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_sendTimer?.Change(Timeout.Infinite, 0);
			_logger.LogInformation($"Stopped at: {DateTimeOffset.Now}");
			return Task.CompletedTask;
		}

		/// <summary>
		/// Фоновая задача
		/// </summary>
		/// <param name="state"></param>
		private async void TimerTask(object state)
		{
			try
			{
				_logger.LogDebug($"send...");
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					using var broadCastService = scope.ServiceProvider.GetRequiredService<BroadcastService>();
					var registeredSenders = scope.ServiceProvider.GetServices<ISender>();
					foreach (var sender in registeredSenders)
					{
						await broadCastService.SendAsync(sender);
					}
				}
				_logger.LogDebug($" done");
			}
			catch (Exception e)
			{
				_logger.LogWarning($"Error while notification sending \r\n{e.Message}");
			}
		}

	}
}
