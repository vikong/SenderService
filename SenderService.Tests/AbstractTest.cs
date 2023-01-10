
using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NTB.SenderService.Data;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
namespace NTB.SenderService.Tests
{
    [TestClass]
    public abstract class AbstractTest : IDisposable
    {
        protected ServiceProvider ServiceProvider { get; set; }

        private IServiceCollection serviceCollection { get; set; }

        [TestInitialize]
        public void TestInitialization()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            /*
            var host = Host
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: false);
                })
                .ConfigureServices((hostContext, services)=> {
                })
                .Build();
            */

            serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddOptions();

            serviceCollection.Configure<TelegramSenderSettings>(config.GetSection("TelegramSenderSettings"));

            serviceCollection.AddDbContext<DatabaseContext>(options =>
            {
                options
                    .UseInMemoryDatabase("Messages")
                    //Отключаем ошибки, что поведение транзакций в InMemory отличается от реляционного
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            
            serviceCollection.AddScoped<TelegramSender>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var dbContext = ServiceProvider.GetService<DatabaseContext>();
            dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            var dbContext = ServiceProvider.GetService<DatabaseContext>();
            dbContext.Dispose();

            serviceCollection = null;
            ServiceProvider = null;
        }

    }
}
