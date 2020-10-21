using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ComplAI.DataLayer.Entity
{
    public class EuDocumentEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("search_term")]
        public string SearchTerm { get; set; }

        [BsonElement("type_of_act")]
        public string TypeOfAct { get; set; }

        public string Title { get; set; }

        [BsonElement("pdf_url")]
        public Uri PdfUrl { get; set; }
    }
}
