using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Options;
using NTB.SenderService.Data;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using NTB.SenderService.TelegramBot;

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
				message.RaiseError(MessageErrorTypeEnum.Provider, $"Wrong recipient's ChatId");
				return false;
			}

			try
			{
				ChatId chat = new ChatId(chatId);

				TelegramMessage msg = ToTelegramMessage(message);
				if (msg != null)
				{
					Telegram.Bot.Types.Message result =
						await _botClient.SendAsync(chatId, msg);

					message.MessageId = $"{chatId}:{result.MessageId}";
					message.StatusId = MessageStatusEnum.Delivered;
				}
				else
				{
					message.RaiseError(MessageErrorTypeEnum.Format,"Empty");
				}
			}
			catch (JsonException ex)
			{
				message.RaiseError(MessageErrorTypeEnum.Format, $"Can't parse JSON: {ex.Message}");
			}
			catch (ApiRequestException ex)
			{
				message.RaiseError(MessageErrorTypeEnum.Provider, $"{ex.GetType()}:{ex.Message}:{ex.ErrorCode}");
			}
			catch (RequestException ex)
			{
				if (ex.InnerException is System.Net.Http.HttpRequestException)
				{
					Exception inner = ex.InnerException;
					message.AppendError(MessageErrorTypeEnum.Network, $"{ex.GetType()}:{ex.Message}:{ex.HttpStatusCode}\r\nInner:{inner.GetType()}:{inner.Message}\r\n{inner?.InnerException.GetType()}:{inner?.InnerException.Message}");
				}
				else
				{
					message.RaiseError(MessageErrorTypeEnum.Request, $"{ex.GetType()}:{ex.Message}:{ex.HttpStatusCode}");
				}
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
				if (strInput.StartsWith("{") && strInput.EndsWith("}"))
				{
					return JsonSerializer.Deserialize<TelegramMessage>(strInput);
				}
				else
				{
					throw new JsonException("Wrong JSON format");
				}
			}

			return new TelegramMessage(message.Text.Trim());

		}
	}
}
