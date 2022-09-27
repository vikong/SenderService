using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTB.SenderService.Data;

namespace NTB.SenderService
{
	public interface ISender
	{
		MessageTypeEnum MessageType { get; }
		Task<Boolean> SendAsync(Message message);
	}
}
