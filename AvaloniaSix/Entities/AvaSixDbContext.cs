using Microsoft.EntityFrameworkCore;

namespace AvaloniaSix.Entities;

public class AvaSixDbContext : DbContext
{
    public DbSet<Setting> Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=avasix.db");
        base.OnConfiguring(optionsBuilder);
    }
}
