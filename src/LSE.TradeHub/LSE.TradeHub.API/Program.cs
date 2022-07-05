using LSE.TradeHub.API.Configuration;
using LSE.TradeHub.Core;
using LSE.TradeHub.Core.Interfaces;
using LSE.TradeHub.Core.Models;
using LSE.TradeHub.Core.Services;
using LSE.TradeHub.Utilities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace LSE.TradeHub.API {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(AutomapperProfile));
            builder.Services.AddLogging(o => o.AddConsole());
            AddLocalServices(builder);

            var seederOptions = new SeederOptions();
            builder.Configuration.GetSection(SeederOptions.Seeder).Bind(seederOptions);

            AddDataSeeding(builder, seederOptions);

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();

            if (seederOptions.SeedData) {
                using(var scope = app.Services.CreateScope()) {
                    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
                    await seeder.SeedData();
                }
            }

            await app.RunAsync();
        }

        static void AddLocalServices(WebApplicationBuilder builder) {
            builder.Services.AddDbContext<TradeDataContext>(o => {
                o.UseInMemoryDatabase(databaseName: "tradedata");
                //o.UseSqlServer(builder.Configuration.GetConnectionString("TradeData"));
            });

            builder.Services.AddScoped<ISystemClock, SystemClock>();
            builder.Services.AddScoped<IStockService, StockService>();
            builder.Services.AddScoped<ITradeRecordService, TradeRecordService>();
        }

        static void AddDataSeeding(WebApplicationBuilder builder, SeederOptions seederOptions) {
            builder.Services.AddSingleton(seederOptions);
            builder.Services.AddScoped<IDataSeeder, DataSeeder>();
            builder.Services.AddScoped<IDataSeeder<Stock>, StockService>();
            builder.Services.AddScoped<IDataSeeder<TradeRecord>, TradeRecordService>();
            builder.Services.AddScoped<IStockLoader, FTSE100Reader>();
            builder.Services.AddScoped<ITradeDataGenerator, TradeDataGenerator>();
        }
    }
}