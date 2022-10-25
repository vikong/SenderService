using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Options;

using NTB.BotService.TelegramBot;
using NTB.SenderService.Data;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

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
		public ITelegramBotClient Bot => _botClient;

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

		private async Task<Int64> GetChatId(Data.Message message)
		{
			if (!Int64.TryParse(message.Recipient, out Int64 chatId))
			{
				return 0;
			}

			return chatId;
		}

		public async Task<Boolean> SendAsync(Data.Message message)
		{
			Int64 chatId = await GetChatId(message);
			if (chatId==0)
			{
				message.SetError(MessageErrorTypeEnum.Provider, $"Wrong recipient's ChatId");
				return false;
			}

			try
			{
				ChatId chat = new ChatId(chatId);

				Telegram.Bot.Types.Message result = 
					await _botClient.SendAsync(chatId, ToTelegramMessage(message));
				
				message.MessageId = $"{chatId}:{result.MessageId}";
				message.StatusId = MessageStatusEnum.Delivered;
			}
			catch (ApiRequestException ex)
			{
				message.SetError(MessageErrorTypeEnum.Provider, $"{ex.Message}:{ex.ErrorCode}");
			}
			catch (RequestException ex)
			{
				message.SetError(MessageErrorTypeEnum.Provider, $"{ex.Message}:{ex.HttpStatusCode}");
			}

			return message.StatusId != MessageStatusEnum.Error;

		}

		/// <summary>
		/// Конвертирует сообщение <see cref="Data.Message"/> в сообщение телеграм <see cref="TelegramMessage"/>
		/// </summary>
		/// <param name="message">Сообщение</param>
		/// <returns>Сообщение телеграм</returns>
		public static TelegramMessage ToTelegramMessage(Data.Message message)
		{
			string strInput = message.Text.Trim();
			if (String.IsNullOrWhiteSpace(strInput))
			{
				return null;
			}
			
			if (message.IsJson)
			{
				TelegramMessage result;
				if (strInput.StartsWith("{") && strInput.EndsWith("}"))
				{
					try
					{
						result = JsonSerializer.Deserialize<TelegramMessage>(strInput);
					}
					catch (JsonException)
					{
						result = null;
					}
				}
				else
				{
					result = null;
				}
				return result;
			}
			return new TelegramMessage(message.Text.Trim());

		}
	}
}
