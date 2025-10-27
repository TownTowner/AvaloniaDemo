using AvaloniaSix.Services;
using System;

namespace AvaloniaSix.Factories;

public class DbFactory(Func<DbService> factory)
{
    public DbService GetDbService(Action<DbService>? afterCreation = null)
    {
        var dbService = factory();

        afterCreation?.Invoke(dbService);

        return dbService;
    }
}
