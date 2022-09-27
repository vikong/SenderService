using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NTB.SenderService.Data.EntityConfigurations
{
	internal class MessageTypeConfiguration
		: IEntityTypeConfiguration<MessageType>
	{
		public void Configure(EntityTypeBuilder<MessageType> builder)
		{
			// table
			builder.ToTable("MessageType");

			// fields
			builder.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(10);

			// indexes
			builder.HasKey(t => t.Id);
			builder.HasIndex("Name").IsUnique();

			// relations

			// datas
			MessageType[] messageTypes = {
				new MessageType {
					Id=MessageTypeEnum.Telegram,
					Name="telegram",
					Attachable=true,
					Subjectable=false,
					MaxLenght=4096,
				}
			};
			builder.HasData(messageTypes);
		}
	}
}
