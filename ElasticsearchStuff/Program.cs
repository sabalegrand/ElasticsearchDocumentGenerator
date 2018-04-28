using System;
using Nest;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using log4net;

namespace ElasticsearchStuff
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        private static string ES_NODE_URL = "http://localhost:9200";
        private static string INDEX_NAME = "test";
        private static string DOCUMENT_TYPE = "document";
        private static int INTERVAL = 10;

        static void Main(string[] args)
        {
            logger.Info("Retrieving docment schema.");
            var documentSchema = GetDocumentSchema(@"F:\Dev\DotNet\Configs\schema_document.txt");
            logger.Info("Document schema retrieved.");

            var documentGenerator = new RandomDocumentGenerator(documentSchema);

            var index = new Index(INDEX_NAME);
            var type = new DocumentType(DOCUMENT_TYPE);
            var listDocument = documentGenerator.GenerateDocuments(1000);
            var continuousInserter = new ContinuousBulkInserter(new Uri(ES_NODE_URL), documentGenerator, index, type, 1);

            continuousInserter.Insert(1000);
        }

        private static Dictionary<string, string> GetDocumentSchema(string schemaFilePath)
        {
            var schema = new Dictionary<string, string>();

            using (StreamReader file = File.OpenText(schemaFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                schema = (Dictionary<string, string>)serializer.Deserialize(file, typeof(Dictionary<string, string>));
            }

            return schema;
        }
    }
}
