using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComplAI.DataLayer.Interfaces;
using MongoDB.Driver;
using ServiceStack;

namespace ComplAI.DataLayer.Repositories
{
    public class CustomCollectionNameMongoDbRepository<TEntity> : MongoDbRepository<TEntity>,
        ICustomMongoRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollectionName<TEntity> _collection;

        public CustomCollectionNameMongoDbRepository(IMongoContext context, IMongoCollectionName<TEntity> collection) : base(context)
        {
            _collection = collection;
        }


        protected override void ConfigDbSet()
        {
            DbSet = Context.GetCollection<TEntity>(_collection.GetCollectionName());
        }
    }
}
