using log4net;
using ElasticsearchStuff.Schema;
using System;

namespace ElasticsearchStuff
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        private static string ES_NODE_URL = "http://localhost:9200";
        private static string INDEX_NAME = "test-data-2018.04.29";
        private static string DOCUMENT_TYPE = "document";

        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(exitHandler);

            logger.Info("Retrieving document schema.");
            var documentSchema = new DocumentSchemaReader(@"F:\Dev\DotNet\Configs\schema_document.txt").GetDocumentSchema();
            logger.Info("Document schema retrieved.");

            var documentGenerator = new RandomDocumentGenerator(documentSchema);

            var index = new Index(INDEX_NAME);
            var type = new DocumentType(DOCUMENT_TYPE);
            var listDocument = documentGenerator.GenerateDocuments(10);
            var continuousInserter = new ContinuousBulkInserter(new Uri(ES_NODE_URL), documentGenerator, index, type, 10);

            continuousInserter.Insert(1000);


        }

        protected static void exitHandler(object sender, ConsoleCancelEventArgs args)
        {
           logger.Info("Indexation has ben canceled.");
           logger.Info($"{ContinuousBulkInserter.IndexDocumentsCount} documents have been indexed.");
        }

    }
}
