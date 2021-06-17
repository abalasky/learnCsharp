﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Play.Common.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {


        private readonly IMongoCollection<T> dbCollection;

        //private readonly FilterDefinitionBuilder<Item> = Builders<Item>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {

            dbCollection = database.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(Builders<T>.Filter.Empty).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(x => x.Id, id);

            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<T> filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);


            await dbCollection.ReplaceOneAsync(filter, entity);

        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(x => x.Id, id);

            await dbCollection.DeleteOneAsync(filter);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        // public async Task<IReadOnlyCollection<IEntity>> GetAsync(Expression<Func<IEntity, bool>> filter)
        // {

        //     return await dbCollection.Find(filter).FirstOrDefaultAsync();
        // }
    }
}
