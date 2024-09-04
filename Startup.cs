using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service;

public class Startup
{
    private ServiceSettings serviceSettings;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

        services.AddMongo().AddMongoRepository<Items>("items");
        
    }

}
