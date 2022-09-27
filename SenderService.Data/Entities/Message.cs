using System;

namespace NTB.SenderService.Data
{
	public class Message : BaseEntity
	{
		/// <summary>
		/// Ключ типа сообщения
		/// </summary>
		public MessageTypeEnum TypeId { get; set; }

		/// <summary>
		/// Тип сообщения
		/// </summary>
		public virtual MessageType Type { get; set; }

		/// <summary>
		/// Адрес получателя
		/// </summary>
		public String Recipient { get; set; }

		/// <summary>
		/// Текст сообщения
		/// </summary>
		public String Text { get; set; }

		/// <summary>
		/// Тема сообщения
		/// </summary>
		public String Subject { get; set; }

		/// <summary>
		/// Вложения
		/// </summary>
		public String AttachesRef { get; set; }

		/// <summary>
		/// Ключ статуса
		/// </summary>
		public MessageStatusEnum StatusId { get; set; }

		/// <summary>
		/// Статус сообщения
		/// </summary>
		public virtual MessageStatus Status { get; set; }

		/// <summary>
		/// Идентификатор сообщения, присвоенный провадером
		/// </summary>
		public String MessageId { get; set; }

		/// <summary>
		/// Время создания
		/// </summary>
		public DateTime Created { get; protected set; }

		/// <summary>
		/// Время обновления
		/// </summary>
		public DateTime Updated { get; set; }

		public override string ToString()
		{
			return @$"	ID:{Id}
	Type:{TypeId}
	To:{Recipient}
	Subject:{Subject}
	{Text}";
		}
	}
}