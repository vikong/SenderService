using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace NTB.SenderService.TelegramBot
{
	public static class TelegramBotExtensions
	{
		public static string MdCharEncode(this string text)
		{
			return text.Replace("_", "\\_")
				.Replace("*", "\\*");
		}

		public static async Task<Message> SendAsync(this ITelegramBotClient botClient, ChatId chatId, TelegramMessage message)
		{
			InlineKeyboardButton[] markup = message.Markup?.ToArray();

			return await botClient.SendTextMessageAsync(
					chatId: chatId,
					text: message.IsPlainText ? message.Text.MdCharEncode() : message.Text,
					parseMode: message.ParseMode,
					replyMarkup: markup != null ? new InlineKeyboardMarkup(message.Markup) : null,
					disableWebPagePreview: message.DisableWebPagePreview
					);
		}

		public static Chat GetChat(this Update update)
		{
			Chat result = update.Type switch
			{
				UpdateType.Message => update.Message.Chat,
				UpdateType.CallbackQuery => update.CallbackQuery.Message.Chat,
				_ => default
			};
			return result;
			//switch (update.Type)
			//{
			//	case UpdateType.InlineQuery:
			//	case UpdateType.ChosenInlineResult:
			//	case UpdateType.EditedMessage:
			//	case UpdateType.ChannelPost:
			//	case UpdateType.EditedChannelPost:
			//	case UpdateType.ShippingQuery:
			//	case UpdateType.PreCheckoutQuery:
			//	case UpdateType.Poll:
			//	case UpdateType.PollAnswer:
			//	case UpdateType.MyChatMember:
			//	case UpdateType.ChatMember:
			//	case UpdateType.ChatJoinRequest:
			//		break;
			//}
		}
	}
}
