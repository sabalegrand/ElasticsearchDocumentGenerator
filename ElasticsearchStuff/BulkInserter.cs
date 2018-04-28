using System;
using System.Collections.Generic;
using Nest;

namespace ElasticsearchStuff
{
    class BulkInserter
    {
        private readonly ElasticClient esClient;

        public BulkInserter(Uri esHost)
        {
            esClient = new ElasticClient(esHost);
            esClient.ClusterHealth();
        }

        public void BulkInsert<T>(List<T> documents, Index index, DocumentType type)
            where T : class
        {
            esClient.IndexMany(documents, index.Value, type.Value);
        }
    }

}
