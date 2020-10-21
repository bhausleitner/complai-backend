using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ComplAI.Resources
{
    public class EuDocumentResource
    {
        public string SearchTerm { get; set; }

        public string TypeOfAct { get; set; }

        public string Title { get; set; }

        public Uri PdfUrl { get; set; }

        public string Id { get; set; }
    }
}
