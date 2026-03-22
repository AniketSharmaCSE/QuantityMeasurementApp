using Microsoft.EntityFrameworkCore;
using QuantityMeasurement.Model.Entities.EF;

namespace QuantityMeasurement.Repository.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MeasurementRecord> Measurements { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeasurementRecord>(e =>
            {
                e.HasIndex(m => m.Operation);
                e.HasIndex(m => m.Operand1Category);
                e.HasIndex(m => m.Timestamp);
            });

            modelBuilder.Entity<UserEntity>(e =>
            {
                e.HasIndex(u => u.Username).IsUnique();
                e.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
