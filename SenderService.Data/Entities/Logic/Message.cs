using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTB.SenderService.Data
{
	public partial class Message
	{
		/// <summary>
		/// Последняя ошибка, возникшая при отправке сообщения
		/// </summary>
		public MessageError LastError => _messageErrors.FirstOrDefault();

		public MessageError SetError(MessageErrorTypeEnum type, String describe)
		{
			MessageError result;
			result = LastError;
			if (result == null)
			{
				result = new MessageError(this.Id, type, describe);
				_messageErrors.Add(result);
			}
			else 
			{
				result.Describe = describe;
				result.Type = type;
			}
			StatusId = MessageStatusEnum.Error;
			return result;
		}

	}
}
