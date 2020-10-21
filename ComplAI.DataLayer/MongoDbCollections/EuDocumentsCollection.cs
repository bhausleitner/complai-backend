using System;
using System.Collections.Generic;
using System.Text;
using ComplAI.DataLayer.Entity;
using ComplAI.DataLayer.Interfaces;

namespace ComplAI.DataLayer.MongoDbCollections
{
    public class EuDocumentsCollection : IMongoCollectionName<EuDocumentEntity>
    {
        public string GetCollectionName()
        {
            return "scrapy_items";
        }
    }
}
