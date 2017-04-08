using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Base;
using Farfetch.Domain.Entities.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Farfetch.Data.MongoDb.Repositories.Base
{
    public abstract class MongoDbRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TEntity, TId>
    {
        #region Fields
        private static IMongoClient client;

        private static IMongoDatabase database;

        private IMongoCollection<TEntity> collection;

        private bool disposed;
        #endregion

        #region Constructors | Destructors
        public MongoDbRepository(string collectionName)
        {
            client = new MongoClient();
            database = client.GetDatabase("test");
            collection = database.GetCollection<TEntity>(collectionName);
        }

        ~MongoDbRepository()
        {
            Dispose(false);
        }

        public Task DeleteAsync(IEnumerable<TEntity> entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IRepository Members
        public virtual async Task<TEntity> GetAsync(TId id)
        {
            return await collection
                .Find(new BsonDocument { { "_id", new ObjectId(id.ToString()) } })
                .FirstAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(string ordering = null, bool ascending = true)
        {
            return await collection
                .Find(new BsonDocument())
                .ToListAsync();
        }

        public virtual Task<PagedList<TEntity>> GetAllAsync(int index, int quantity, string ordering = null, bool ascending = true)
        {
            throw new NotImplementedException();
        }

        ////public List<PostModel> Filter(string jsonQuery)
        ////{
        ////    var queryDoc = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
        ////    return _collection.Find<PostModel>(queryDoc).ToList();
        ////}

        public virtual Task<int> GetAllPagedItemsAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            if (entity.IsTransient())
            {
                await collection.InsertOneAsync(entity, null);
                return await GetAsync(entity.Id);
            }
            else
            {
                ////entity.Id = new ObjectId(entity.Id);

                var filter = Builders<TEntity>.Filter.Eq(s => s.Id, entity.Id);
                await collection.ReplaceOneAsync(filter, entity, null);
                return await GetAsync(entity.Id);
            }
        }
        #endregion

        #region Disposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
            }

            disposed = true;
        }
        #endregion
    }
}