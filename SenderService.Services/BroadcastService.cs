using System;
using System.Threading.Tasks;
using System.Linq;
using NTB.SenderService.Data;
using Microsoft.EntityFrameworkCore;

namespace NTB.SenderService
{
	public class BroadcastService: IDisposable
	{
		private readonly DatabaseContext _dbContext;

		public BroadcastService(DatabaseContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}

		public async Task SendAsync(ISender sender)
		{
			// получаем неотправленные
			Message[] queuedMessages = await _dbContext.Messages
				.Where(m => m.TypeId==sender.MessageType && m.StatusId == MessageStatusEnum.Queued)
				.ToArrayAsync();
			if (queuedMessages.Count() == 0)
			{
				return;
			}

			// отправляем все
			foreach (Message message in queuedMessages)
			{
				// пометим, что отправляется
				message.StatusId = MessageStatusEnum.Dispatch;
				message.Updated = DateTime.UtcNow;
				try
				{
					_dbContext.Update(message);
					await _dbContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					// параллельный процесс первым начал отправку, отменяем
					_dbContext.Entry<Message>(message).State = EntityState.Detached;
					continue;
				}

				//отправляем
				try
				{
					await sender.SendAsync(message);
				}
				catch (Exception ex)
				{
					message.SetError(MessageErrorTypeEnum.Program, ex.Message);
				}

				// обновляем статус
				try
				{
					message.Updated = DateTime.UtcNow;
					_dbContext.Update(message);
					await _dbContext.SaveChangesAsync();
				}
				catch
				{
					// DbUpdateConcurrencyException тут быть не должно. 
					continue;
				}
			}

		}
	}
}
