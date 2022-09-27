using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NTB.SenderService.Data.EntityConfigurations
{
	internal class MessageStatusConfiguration
		: IEntityTypeConfiguration<MessageStatus>
	{
		public void Configure(EntityTypeBuilder<MessageStatus> builder)
		{
			// table
			builder.ToTable("MessageStatus");

			// fields
			builder.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(25);

			// indexes
			builder.HasKey(t => t.Id);
			builder.HasIndex("Name").IsUnique();

			// relations

			// datas
			MessageStatus[] messageStatuses = {
				new MessageStatus { Id=MessageStatusEnum.Queued, Name="Поставлено в очередь" },
				new MessageStatus { Id=MessageStatusEnum.Dispatch, Name="В процессе отправки" },
				new MessageStatus { Id=MessageStatusEnum.Submitted, Name="Передано провайдеру" },
				new MessageStatus { Id=MessageStatusEnum.Delivered, Name="Доставлено получателю" },
				new MessageStatus { Id=MessageStatusEnum.Error, Name="Ошибка доставки" },
			};
			builder.HasData(messageStatuses);
		}
	}
}
