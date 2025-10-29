using Microsoft.EntityFrameworkCore;

namespace AvaloniaSix.Entities;

public class AvaSixDbContext : DbContext
{
    public DbSet<ActionPrintSettings> ActionPrintSettingses { get; set; }
    public DbSet<ActionPrintSettingsProfile> ActionPrintSettingsProfiles { get; set; }
    public DbSet<ActionTabPrint> ActionTabPrints { get; set; }

    public DbSet<Setting> Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=avasix.db");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Actions (Concrete type)
        //modelBuilder.Entity<Action>()
        //    .UseTpcMappingStrategy()
        //    .HasKey(f => f.Id);

        
        // Print Settings Profile
        modelBuilder.Entity<ActionPrintSettingsProfile>()
            .HasOne(f => f.ActionPrintSettings)
            .WithMany(f => f.ActionPrintSettingsProfiles)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Print Settings
        modelBuilder.Entity<ActionPrintSettings>()
            .HasMany(f => f.ActionTabPrints)
            .WithOne(f => f.ActionPrintSettings)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Process Actions
        //modelBuilder.Entity<Process>()
        //    .HasMany(f => f.Actions)
        //    .WithOne(f => f.Process)
        //    .HasForeignKey(f => f.ProcessId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}
