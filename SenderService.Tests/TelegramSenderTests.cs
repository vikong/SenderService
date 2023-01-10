using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json.Serialization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using NTB.SenderService.TelegramBot;
using System;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NTB.SenderService.Tests
{
	[TestClass]
	public class TelegramSenderTests : AbstractTest
	{
		private readonly long ChatId = 900984646;

		private TelegramSender CreateSender() 
			=> ServiceProvider.GetRequiredService<TelegramSender>();

		[TestMethod]
		public void TestConfig()
		{
			//TelegramSenderSettings conf = serviceCollection.Get<TelegramSenderSettings>();
			var opt = ServiceProvider.GetService<IOptions<TelegramSenderSettings>>();
			Console.WriteLine(opt);
			Console.WriteLine(opt.Value.Token);
			

		}

		[TestMethod]
		public async Task TelegramBot_SendMessage_Succesfully()
		{
			string text = "Вам задача 62557:\n<b>БРОКЕР ХЕЛЛМАНН</b>";
			var keyboard = new InlineKeyboardMarkup(new[]
			{
				new [] {
					InlineKeyboardButton.WithUrl("Перейти",@"http://reportal.ntbroker.ru/itilium/itiledit.php?NUMINC=62557&MODE=1"),
					InlineKeyboardButton.WithCallbackData("Показать"),
				}
			});
			
			var sender = CreateSender();

			await sender.Bot.SendTextMessageAsync(ChatId, text,
				parseMode: ParseMode.Html,
				replyMarkup: keyboard,
				disableWebPagePreview: true);
		}

		[TestMethod]
		public async Task TelegramSender_SendMessage_Succesfully()
		{
			TelegramMessage message = new TelegramMessage("Hi! Это тестовое сообщение")
			{
				Buttons = new TelegramButton[] { TelegramButton.Url("Ya", @"https://ya.ru/") }
			};

			var sender = CreateSender();

			await sender.Bot.SendAsync(new ChatId(ChatId), message);
		}

		[TestMethod]
		public void ConvertJsonMessage_ToTelegram_Succeed()
		{
			string json = @"{
""Text"":""test message\nwith new line"",
""ParseAs"":""txt"", 
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

			var message = TelegramSender.ToTelegramMessage(new Data.Message()
			{
				Id = 1,
				IsJson = true,
				Text = json
			});

			Assert.IsNotNull(message);
			Assert.AreEqual("test message\nwith new line", message.Text);
			Assert.IsTrue(message.IsPlainText);
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

		[TestMethod]
		public async Task SendJsonMessage_ToTelegram_Succeed()
		{
			string json = @"{ 
""Text"":""Новая задача:62680\nRe:**62679** ПИРАМИДА _БЭСТ-БЮРО"", 
""Buttons"":[ 
	{ ""Type"":2, ""Text"":""Открыть"", ""Data"":""http://reportal.ntbroker.ru/itilium/itiledit.php?NUMINC=62680&MODE=1"" }, 
	{ ""Type"":1, ""Text"":""Посмотреть"", ""Data"":""/62680"" } 
] }";

			var message = TelegramSender.ToTelegramMessage(new Data.Message()
			{
				Id = 1,
				IsJson = true,
				Text = json
			});

			var sender = CreateSender();

			await sender.Bot.SendAsync(new ChatId(ChatId), message);

		}

		[TestMethod]
		public async Task SendJsonFile_ToTelegram_Succeed()
		{
			string json = System.IO.File.ReadAllText(@"D:\temp\message.json");

			var message = TelegramSender.ToTelegramMessage(new Data.Message()
			{
				Id = 1,
				IsJson = true,
				Text = json
			});

			var sender = CreateSender();

			await sender.Bot.SendAsync(new ChatId(ChatId), message);
		}
	}
}