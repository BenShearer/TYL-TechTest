using LSE.TradeHub.API.Configuration;
using LSE.TradeHub.Core;
using LSE.TradeHub.Core.Interfaces;
using LSE.TradeHub.Core.Services;

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

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();

            await app.RunAsync();
        }

        static void AddLocalServices(WebApplicationBuilder builder) {
            builder.Services.AddDbContext<TradeDataContext>(o => {
                o.UseInMemoryDatabase(databaseName: "tradedata");
            });

            builder.Services.AddScoped<ISystemClock, SystemClock>();
            builder.Services.AddScoped<IStockService, StockService>();
            builder.Services.AddScoped<ITradeRecordService, TradeRecordService>();
        }
    }
}