using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NTB.SenderService.Data.EntityConfigurations
{
	internal class MessageConfiguration
		: IEntityTypeConfiguration<Message>
	{
		public void Configure(EntityTypeBuilder<Message> builder)
		{
			// table
			builder.ToTable("Messages");

			// fields
			builder.Property(t => t.StatusId)
				.IsRequired()
				.HasDefaultValueSql("1");

			builder.Property(t => t.Text)
				.IsRequired();

			builder.Property(t => t.Subject)
				.HasMaxLength(255)
				.IsRequired(false);

			builder.Property(e => e.Created)
				.HasDefaultValueSql("getutcdate()");

			builder.Property(e => e.Updated)
				.HasDefaultValueSql("getutcdate()");

			// indexes
			builder.HasKey(t => t.Id);

			// relations
			builder.HasOne(t => t.Status)
				.WithMany(t => t.Messages)
				.HasForeignKey(t=>t.StatusId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(t => t.Type)
				.WithMany(t => t.Messages)
				.HasForeignKey(t => t.TypeId)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
