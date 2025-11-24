using Microsoft.EntityFrameworkCore;
using Domain;

namespace ITService.Data.SqlServer
{
    public class ITServiceDbContext : DbContext  
    {
        public ITServiceDbContext(DbContextOptions<ITServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка сущности Request
            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Tipe).IsRequired().HasMaxLength(100);
                entity.Property(r => r.Model).IsRequired().HasMaxLength(200);
                entity.Property(r => r.Description).IsRequired();
                entity.Property(r => r.Status).IsRequired().HasMaxLength(50);
                entity.Property(r => r.ClientFullName).IsRequired().HasMaxLength(200);
                entity.Property(r => r.ClientPhone).IsRequired().HasMaxLength(20);
                entity.Property(r => r.Engineer).HasMaxLength(100);
            });

            // Настройка сущности User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.FIO);
                entity.Property(u => u.FIO).IsRequired().HasMaxLength(200);
                entity.Property(u => u.Phone_Number).IsRequired().HasMaxLength(20);
            });
        }
    }
}
