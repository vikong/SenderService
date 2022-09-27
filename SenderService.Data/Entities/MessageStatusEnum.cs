namespace NTB.SenderService.Data
{
	/// <summary>
	/// Статус сообщения
	/// </summary>
	public enum MessageStatusEnum
	{
		/// <summary>
		/// Поставлено в очередь
		/// </summary>
		Queued = 1,

		/// <summary>
		/// В процессе отправки
		/// </summary>
		Dispatch = 2,

		/// <summary>
		/// Передано провайдеру
		/// </summary>
		Submitted = 3,

		/// <summary>
		/// Доставлено получателю
		/// </summary>
		Delivered = 4,

		/// <summary>
		/// Отмена пользователем
		/// </summary>
		Cancelled = 8,

		/// <summary>
		/// Ошибка доставки
		/// </summary>
		Error = 9
	}
}