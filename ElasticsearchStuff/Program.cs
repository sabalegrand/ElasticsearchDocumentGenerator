using System;
using Nest;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ElasticsearchStuff
{
    class Program
    {
        private static string ES_NODE_URL = "http://localhost:9200";

        static void Main(string[] args)
        {
            var documentSchema = GetDocumentSchema(@"F:\Dev\DotNet\Configs\schema_document.txt");

            var documentGenerator = new RandomDocumentGenerator(documentSchema);
            var documentList = documentGenerator.GenerateDocuments(100);

            var esClient = new ElasticClient(new Uri(ES_NODE_URL));
            esClient.ClusterHealth();

            var listDocument = documentGenerator.GenerateDocuments(1000);

            esClient.IndexMany(listDocument.Select(doc => doc.AsDictionary()), "test", "Type");
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
