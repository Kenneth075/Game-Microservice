using System;

namespace Play.Catalog.Service.Settings;

public class MongoDbSettings
{
    public string Host { get; init; }
    public int Port { get; init; }

    public string connectionString => $"mongodb://{Host}:{Port}";

}
