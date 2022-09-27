using System;
using System.Collections.Generic;

namespace NTB.SenderService.Data
{
	public class MessageType : BaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public new MessageTypeEnum Id { get; set; }
		
		/// <summary>
		/// Messages type name
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// Ability to have a subject
		/// </summary>
		public Boolean Subjectable { get; set; }

		/// <summary>
		/// Ability to have the attaches
		/// </summary>
		public Boolean Attachable { get; set; }
		
		/// <summary>
		/// Maximum possible length of messages body
		/// </summary>
		public Int32 MaxLenght { get; set; }

		/// <summary>
		/// Messages
		/// </summary>
		public virtual ICollection<Message> Messages { get; set; }

	}
}