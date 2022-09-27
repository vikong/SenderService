using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTB.SenderService.Data
{
	/// <summary>
	/// Разрешённые типы сообщений
	/// </summary>
	public enum MessageTypeEnum
	{
		Email = 1,
		Sms = 2,
		Ftp = 3,
		/// <summary>
		/// Телеграм
		/// </summary>
		Telegram = 4
	}

}
