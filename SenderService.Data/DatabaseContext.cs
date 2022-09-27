using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace NTB.SenderService.Data
{
	public class DatabaseContext: DbContext
	{
		public DbSet<MessageStatus> MessageStatuses { get; set; }
		public DbSet<MessageType> MessageTypes { get; set; }
		public DbSet<Message> Messages { get; set; }

		public DatabaseContext(DbContextOptions options)
			:base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			//modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			//modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

			//modelBuilder.Configurations.Add(new MessageTypeConfiguration());
			//modelBuilder.Configurations.Add(new MessageStatusConfiguration());
			//modelBuilder.Configurations.Add(new MessageConfiguration());
		}
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder
		//		.LogTo(message => Debug.WriteLine(message))
		//		.EnableSensitiveDataLogging()
		//		.EnableDetailedErrors();
		//}
	}
}
