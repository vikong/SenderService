using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NTB.SenderService.Data.EntityConfigurations
{
	internal class MessageErrorConfiguration
		: IEntityTypeConfiguration<MessageError>
	{
		public void Configure(EntityTypeBuilder<MessageError> builder)
		{
			// table
			builder.ToTable("MessageError");

			// fields
			builder.Property(t => t.Describe)
				.IsRequired();

			builder.Property(e => e.Created)
				.HasDefaultValueSql("getutcdate()");

			// indexes
			builder.HasKey(t => t.Id);

			// relations
			builder.HasOne(t => t.Message)
				.WithMany(t=>t.MessageErrors)
				.HasForeignKey(t=>t.MessageId)
				.OnDelete(DeleteBehavior.Cascade);

		}
	}
}
