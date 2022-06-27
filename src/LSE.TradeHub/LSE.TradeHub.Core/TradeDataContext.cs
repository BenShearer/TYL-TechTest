using LSE.TradeHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LSE.TradeHub.Core {
    public class TradeDataContext : DbContext {
        public DbSet<Stock?> Stocks { get; set; }
        public DbSet<TradeRecord> TradeRecords { get; set; }

        public TradeDataContext(DbContextOptions<TradeDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<Stock>().HasMany(x => x.TradeRecords);
        }
    }
}
