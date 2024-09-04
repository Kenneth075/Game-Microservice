using System;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public class ItemsRepository
{
    private const string collectionsName = "Items";
    private readonly IMongoCollection<Items> dbConnection;
    private readonly FilterDefinitionBuilder<Items> filterBuilder = Builders<Items>.Filter;

    public ItemsRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("Catalog");
        dbConnection = database.GetCollection<Items>(collectionsName);

    }

    public async Task<IReadOnlyCollection<Items>> GetAllAsync()
    {
        return await dbConnection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<Items> GetByIdAsync(Guid id)
    {
        FilterDefinition<Items> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbConnection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Items item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        await dbConnection.InsertOneAsync(item);
    }

    public async Task UpdateAsync(Items item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        FilterDefinition<Items> filter = filterBuilder.Eq(exitingEntity => exitingEntity.Id, item.Id);
        await dbConnection.ReplaceOneAsync(filter, item);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = filterBuilder.Eq(entity => entity.Id, id);
        await dbConnection.DeleteOneAsync(filter);
    }

}
