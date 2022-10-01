using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NTB.SenderService.Data;
using Telegram.Bot;
using TB = Telegram.Bot;

namespace NTB.SenderService
{
	public class TelegramSenderSettings
	{
		public String Token { get; set; }
		public Boolean TestMode { get; set; }
	}


	public class TelegramSender : ISender
	{
		public MessageTypeEnum MessageType => MessageTypeEnum.Telegram;

		private readonly ITelegramBotClient _botClient;

		public TelegramSender(IOptions<TelegramSenderSettings> settings)
		{
			if (String.IsNullOrWhiteSpace(settings.Value?.Token))
			{
				throw new ArgumentException($"Empty Telegram Bot token. Define token in appsettings.json");
			}

			_botClient=new TelegramBotClient(new TelegramBotClientOptions(
				settings.Value.Token, 
				useTestEnvironment: settings.Value.TestMode)
				);
		}

		public TelegramSender(ITelegramBotClient telegramBotClient)
		{
			_botClient = telegramBotClient ?? throw new ArgumentNullException(nameof(telegramBotClient));
		}

		private async Task<Int64> GetChatId(Message message)
		{
			if (!Int64.TryParse(message.Recipient, out Int64 chatId))
			{
				return 0;
			}

			return chatId;
		}

		public async Task<Boolean> SendAsync(Message message)
		{
			Int64 chatId = await GetChatId(message);
			if (chatId==0)
			{
				message.StatusId = MessageStatusEnum.Error;
				return false;
			}
			bool result;
			try
			{
				var chat = new TB.Types.ChatId(chatId);
				TB.Types.Message res = await _botClient.SendTextMessageAsync(chat, message.Text);
				message.MessageId = $"{chatId}:{res.MessageId}";
				message.StatusId = MessageStatusEnum.Delivered;
#if DEBUG
				var json = System.Text.Json.JsonSerializer.Serialize(res, typeof(TB.Types.Message),
					new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, WriteIndented = true });
				System.IO.File.AppendAllText(@"D:\temp\__Bot.txt", json + Environment.NewLine);
#endif
				result = true;
			}
			catch (Exception ex)
			{
				message.StatusId = MessageStatusEnum.Error;
				result = false;
			}

			return result;
		}
	}
}
