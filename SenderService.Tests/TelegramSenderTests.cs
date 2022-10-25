using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTB.SenderService.Data;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using NTB.BotService.TelegramBot;
using System.Linq;

namespace NTB.SenderService.Tests
{
	[TestClass]
	public class TelegramSenderTests : AbstractTest
	{
		private readonly TelegramSender sender;
		private readonly long ChatId = 900984646;

		public TelegramSenderTests()
		{
			TelegramSenderSettings telegramSettings = new TelegramSenderSettings()
			{
				Token = "5771418221:AAH2XLiARrnChTPS4KJvAvKO9Eh6450Lcx8",
				TestMode = false
			};
			IOptions<TelegramSenderSettings> options = Options.Create(telegramSettings);

			sender = new TelegramSender(options);
		}

		[TestMethod]
		public async Task TelegramBot_SendMessage_Succesfully()
		{
			string text = "Вам задача 62557:\n<b>БРОКЕР ХЕЛЛМАНН</b>";
			var keyboard = new InlineKeyboardMarkup(new[]
{
	new [] // first row
    {
		InlineKeyboardButton.WithUrl("Перейти",@"http://reportal.ntbroker.ru/itilium/itiledit.php?NUMINC=62557&MODE=1"),
		InlineKeyboardButton.WithCallbackData("Показать"),
	}
});
			await sender.Bot.SendTextMessageAsync(ChatId, text,
				parseMode: ParseMode.Html,
				replyMarkup: keyboard,
				disableWebPagePreview:true);
		}

		[TestMethod]
		public async Task TelegramSender_SendMessage_Succesfully()
		{
			TelegramMessage message = new TelegramMessage("Hi! Это тестовое сообщение")
			{
				Buttons = new TelegramButton[] { TelegramButton.Url("Ya", @"https://ya.ru/") }
			};

			await sender.Bot.SendAsync(new ChatId(ChatId), message);
		}

		[TestMethod]
		public void ConvertJsonMessage_ToTelegram_Succeed()
		{
			string json = @"{
""Text"":""test message"",
""ParseMode"":1,
""Buttons"":[
{
	""Type"":2,
	""Text"":""Ya"",
	""Data"":""https://ya.ru/""
},
{
	""Type"":1,
	""Text"":""Get"",
	""Data"":""123""
}
],
""DisableWebPagePreview"":true
}";

			var message = TelegramSender.ToTelegramMessage(new Data.Message() { 
				Id=1, 
				IsJson=true, 
				Text=json });

			Assert.IsNotNull(message);
			Assert.AreEqual("test message", message.Text);
			Assert.IsNotNull(message.Buttons);
			Assert.AreEqual(2, message.Buttons.Count());
		}

		[TestMethod]
		[DataRow("")]
		[DataRow("simple text")]
		[DataRow(@"{{""Text""}:""test message"",}")]
		public void ConvertMessage_ToTelegram_Fault(string json)
		{

			var message = TelegramSender.ToTelegramMessage(new Data.Message()
			{
				Id = 1,
				IsJson = true,
				Text = json
			});

			Assert.IsNull(message);
		}
	}
}