using System;
using System.Collections.Generic;

namespace NTB.SenderService.Data
{
	public class MessageStatus : BaseEntity
	{
		public new MessageStatusEnum Id { get; set; }
		public String Name { get; set; }

		public virtual ICollection<Message> Messages { get; set; }
	}
}