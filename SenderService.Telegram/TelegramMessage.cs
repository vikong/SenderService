using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NTB.SenderService.TelegramBot
{
	public enum TelegramButtonTypeEnum
	{
		Callback = 1,
		Url = 2
	}

	public class TelegramButton
	{
		public TelegramButtonTypeEnum Type { get; set; }
		public String Text { get; set; }
		public String Data { get; set; }

		[Obsolete("Only for Json", true)]
		public TelegramButton()
		{ }

		public TelegramButton(TelegramButtonTypeEnum type, String text)
		{
			Type = type;
			Text = text;
		}

		public static TelegramButton Url(String text, String url)
			=> new TelegramButton(TelegramButtonTypeEnum.Url, text) { Data = url };

		public static TelegramButton Callback(String text, String data)
			=> new TelegramButton(TelegramButtonTypeEnum.Callback, text) { Data = data };
	}

	public class TelegramMessage
	{
		public String Text { get; set; }
		public String ParseAs { get; set; }

		public Boolean IsPlainText =>
			String.IsNullOrWhiteSpace(ParseAs)
			? true
			: ParseAs.Trim().ToUpper() switch
			{
				"TXT" => true,
				"TEXT" => true,
				"HTML" => false,
				"HTM" => false,
				"MD" => false,
				_ => true
			};

		[JsonIgnore]
		public ParseMode ParseMode =>
			String.IsNullOrWhiteSpace(ParseAs)
			? ParseMode.Markdown
			: ParseAs.Trim().ToUpper() switch
			{
				"TXT" => ParseMode.Markdown,
				"TEXT" => ParseMode.Markdown,
				"HTML" => ParseMode.Html,
				"HTM" => ParseMode.Html,
				"MD" => ParseMode.Markdown,
				_ => ParseMode.Markdown
			};



		public IEnumerable<TelegramButton> Buttons { get; set; }

		[JsonIgnore]
		public IEnumerable<InlineKeyboardButton> Markup
		{
			get
			{
				if (Buttons != null && Buttons.Any())
				{
					var inlineKeyboardButtons = new List<InlineKeyboardButton>(Buttons.Count());
					foreach (var item in Buttons)
					{
						inlineKeyboardButtons.Add(ToInlineKeyboardButton(item));
					}
					return inlineKeyboardButtons;
				}
				else
				{
					return null;
				};
			}
		}

		public Boolean DisableWebPagePreview { get; set; } = true;

		[Obsolete("Only for Json", true)]
		public TelegramMessage()
		{ }

		public TelegramMessage(String text)
		{
			Text = text;
			Buttons = new List<TelegramButton>();
		}

		public static InlineKeyboardButton ToInlineKeyboardButton(TelegramButton button)
		{
			return button.Type switch
			{
				TelegramButtonTypeEnum.Callback
					=> InlineKeyboardButton.WithCallbackData(button.Text, button.Data),

				TelegramButtonTypeEnum.Url
					=> InlineKeyboardButton.WithUrl(button.Text, button.Data),

				_ => new InlineKeyboardButton(button.Text),
			};
			//InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(button.Text, button.Data)
		}
	}
}
