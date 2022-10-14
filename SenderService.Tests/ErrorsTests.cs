using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NTB.SenderService.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace NTB.SenderService.Tests
{
	[TestClass]
	public class ErrorsTests: AbstractTest
	{
		[TestMethod]
		public async Task AddError_ToMessage_Successful()
		{
			DatabaseContext ctx = ServiceProvider.GetService<DatabaseContext>();
			var messages = ctx.Messages;
			var msg = new Message {
				Recipient="TestSystem", 
				Subject="Test Error", 
				TypeId= MessageTypeEnum.Telegram 
			};
			messages.Add(msg);
			await ctx.SaveChangesAsync();
			var msgId = msg.Id;
			var msgWithError = messages.FirstOrDefault(m => m.Id == msgId);
			var err = msgWithError.SetError(MessageErrorTypeEnum.Program, "test error");
			await ctx.SaveChangesAsync();
			var savedErr = ctx.MessageErrors.FirstOrDefault(t => t.MessageId == msgId);

			Assert.IsNotNull(savedErr);
			Assert.AreEqual(err.Describe, savedErr.Describe);

		}

		[TestMethod]
		public async Task ChangeError_InMessage_ChangesError()
		{
			DatabaseContext ctx = ServiceProvider.GetService<DatabaseContext>();
			var messages = ctx.Messages;
			var msg = new Message
			{
				Recipient = "TestSystem",
				Subject = "Test Error",
				TypeId = MessageTypeEnum.Telegram
			};
			messages.Add(msg);
			await ctx.SaveChangesAsync();

			var msgId = msg.Id;
			var errorText = "this is test error";
			msg.SetError(MessageErrorTypeEnum.Program, errorText);
			await ctx.SaveChangesAsync();
			var msgWithError = messages.FirstOrDefault(m => m.Id == msgId);
			var savedErr = ctx.MessageErrors.FirstOrDefault(t => t.MessageId == msgId);

			Assert.IsNotNull(savedErr);
			Assert.AreEqual(errorText, savedErr.Describe);

		}

		[TestMethod]
		public async Task LastError_Returns_Error()
		{
			DatabaseContext ctx = ServiceProvider.GetService<DatabaseContext>();
			var messages = ctx.Messages;
			var msg = new Message
			{
				Recipient = "TestSystem",
				Subject = "Test Error",
				TypeId = MessageTypeEnum.Telegram
			};
			var errorText = "this is test error";
			msg.SetError( MessageErrorTypeEnum.Program, errorText);
			messages.Add(msg);
			await ctx.SaveChangesAsync();
			var msgId = msg.Id;

			var msgWithError = messages.FirstOrDefault(m => m.Id == msgId);
			
			Assert.AreEqual(errorText, msgWithError.LastError.Describe);

		}
	}
}
