namespace NTB.SenderService.Data
{
	/// <summary>
	/// Типы ошибок отправки сообщений
	/// </summary>
	public enum MessageErrorTypeEnum
	{
		/// <summary>
		/// Программная ошибка
		/// </summary>
		Program = 1,

		/// <summary>
		/// Ошибка провайдера
		/// </summary>
		Provider = 2
	}
}