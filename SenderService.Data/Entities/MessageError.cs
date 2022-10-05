using System;

namespace NTB.SenderService.Data
{
	/// <summary>
	/// Запись об ошибках отправки
	/// </summary>
	public class MessageError : BaseEntity
	{
		/// <summary>
		/// Описание ошибки
		/// </summary>
		public String Describe { get; set; }

		/// <summary>
		/// Тип ошибки
		/// </summary>
		public MessageErrorTypeEnum Type { get; set; }

		/// <summary>
		/// Ключ сообщения, к которому относится запись об ошибке
		/// </summary>
		public Int32 MessageId { get; set; }

		/// <summary>
		/// Сообщение, к которому относится запись об ошибке
		/// </summary>
		public virtual Message Message { get; protected set; }

		/// <summary>
		/// Время добавления
		/// </summary>
		public DateTime Created { get; protected set; }

		[Obsolete("Only for CRM", true)]
		public MessageError()
		{ }

		public MessageError(Int32 messageId, MessageErrorTypeEnum errorType, String describe)
		{
			MessageId = messageId;
			Type = errorType;
			Describe = describe;
		}
	}
}