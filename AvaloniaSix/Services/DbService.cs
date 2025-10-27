using AvaloniaSix.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSix.Services;

public class DbService : IDisposable
{
    private AvaSixDbContext _dbContext;

    public DbService(AvaSixDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ApplyMigrations()
    {
        _dbContext.Database.EnsureCreated();

        //_dbContext.Database.Migrate();
    }

    public Setting GetSetting()
    {
        var setting = _dbContext.Settings.FirstOrDefault();
        if (setting != null) return setting;

        setting = new()
        {
            LocationPaths = ["Initial Path1", "Initial Path2", "Initial Path3"]
        };
        SaveSetting(setting);

        return setting;
    }

    public void SaveSetting(Setting setting)
    {
        _dbContext.Settings.RemoveRange(_dbContext.Settings);

        _dbContext.Settings.Add(setting);
        _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}
