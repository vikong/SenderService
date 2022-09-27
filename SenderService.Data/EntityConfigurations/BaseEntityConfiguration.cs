using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NTB.SenderService.Data.EntityConfigurations
{
	internal class BaseEntityConfiguration<T>
		: IEntityTypeConfiguration<T> where T : BaseEntity
	{
		public void Configure(EntityTypeBuilder<T> builder)
		{
			// Primary Key
			builder.HasKey(t => t.Id);
		}
	}
}


