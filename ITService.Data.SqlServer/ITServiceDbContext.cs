using Data.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace RepairService.Data.SqlServer;

public class RepairServiceDbContext : DbContext
{
    public RepairServiceDbContext(DbContextOptions<RepairServiceDbContext> options) : base(options)
    {
    }

    public DbSet<Request> Requests { get; set; }
    public DbSet<User> Users { get; set; }
}