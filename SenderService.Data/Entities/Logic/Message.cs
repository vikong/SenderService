using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTB.SenderService.Data
{
	public partial class Message // логика
	{
		/// <summary>
		/// Последняя ошибка, возникшая при отправке сообщения
		/// </summary>
		public MessageError LastError => _messageErrors.FirstOrDefault();

		protected MessageError SetError(MessageErrorTypeEnum type, MessageStatusEnum messageStatus, String describe)
		{
			MessageError result;
			result = LastError;
			if (result == null)
			{
				result = new MessageError(Id, type, describe);
				_messageErrors.Add(result);
			}
			else
			{
				result.Describe = describe;
				result.Type = type;
			}
			StatusId = messageStatus;
			return result;
		}

		/// <summary>
		/// Добавляет информацию об ошибке и устанавливает статус, соответствующий ошибке доставки
		/// </summary>
		/// <param name="type"></param>
		/// <param name="describe"></param>
		/// <returns></returns>
		public MessageError RaiseError(MessageErrorTypeEnum type, String describe) 
			=> SetError(type, MessageStatusEnum.Error, describe);

		public MessageError AppendError(MessageErrorTypeEnum type, String describe)
			=> SetError(type, MessageStatusEnum.Warning, describe);

	}
}
