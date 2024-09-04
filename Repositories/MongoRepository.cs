using System;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> dbConnection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionsName)
    {
        dbConnection = database.GetCollection<T>(collectionsName);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await dbConnection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbConnection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        await dbConnection.InsertOneAsync(item);
    }

    public async Task UpdateAsync(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        FilterDefinition<T> filter = filterBuilder.Eq(exitingEntity => exitingEntity.Id, item.Id);
        await dbConnection.ReplaceOneAsync(filter, item);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = filterBuilder.Eq(entity => entity.Id, id);
        await dbConnection.DeleteOneAsync(filter);
    }

}
