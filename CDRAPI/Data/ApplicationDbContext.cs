using Domain.LookupModels;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CDRAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Currency> Currencies { get; set; }

        public virtual DbSet<CallRecord> CallRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CallRecord>().HasOne(x => x.Currencys).WithMany().HasForeignKey("Currency");
            modelBuilder.Entity<Currency>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<CallRecord>(entity =>
            {
                entity.Property(x => x.Cost).HasPrecision(18, 3);
                entity.HasIndex(x => x.CallDate);
                entity.HasIndex(x => x.CallerId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
