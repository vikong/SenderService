using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NTB.SenderService.Data;

namespace NTB.SenderService
{
	public class FileSenderSettings
	{
		public String LogFile { get; set; }
	}

	public class FileSender : ISender
	{
		private readonly FileSenderSettings _settings;

		public MessageTypeEnum MessageType => MessageTypeEnum.Telegram;

		public FileSender(IOptions<FileSenderSettings> settings)
		{
			_settings = settings.Value ?? throw new ArgumentNullException(nameof(settings.Value));
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public async Task<Boolean> SendAsync(Message message)
		{
			await File.AppendAllTextAsync(_settings.LogFile, $"-send at {DateTime.Now}{Environment.NewLine}{message}{Environment.NewLine}");
			message.StatusId = MessageStatusEnum.Delivered;
			return true;
		}
	}
}