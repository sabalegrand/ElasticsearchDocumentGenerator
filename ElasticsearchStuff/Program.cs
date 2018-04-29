using log4net;
using ElasticsearchStuff.Schema;
using System;

namespace ElasticsearchStuff
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        private static string ES_NODE_URL = "http://localhost:9200";
        private static string INDEX_NAME = "test";
        private static string DOCUMENT_TYPE = "document";

        static void Main(string[] args)
        {
            logger.Info("Retrieving docment schema.");
            var documentSchema = new DocumentSchemaReader(@"F:\Dev\DotNet\Configs\schema_document.txt").GetDocumentSchema();
            logger.Info("Document schema retrieved.");

            var documentGenerator = new RandomDocumentGenerator(documentSchema);

            var index = new Index(INDEX_NAME);
            var type = new DocumentType(DOCUMENT_TYPE);
            var listDocument = documentGenerator.GenerateDocuments(1000);
            var continuousInserter = new ContinuousBulkInserter(new Uri(ES_NODE_URL), documentGenerator, index, type, 1);

            continuousInserter.Insert(1000);
        }
    }
}
