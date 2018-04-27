using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElasticsearchStuff.ValueGenerator;

namespace ElasticsearchStuff
{

    enum FieldType
    {
        String,
        Integer,
        Float,
        Date
    }

    class RandomDocumentGenerator
    {
        private Dictionary<string, string> documentSchema;

        public RandomDocumentGenerator(Dictionary<string, string> documentSchema)
        {
            this.documentSchema = documentSchema;
        }

        public Document GenerateDocument()
        {
            Document document = new Document();

            foreach (var schema in documentSchema)
            {
                document.AddField(GenerateField(schema.Key, schema.Value));
            }
             
            return document;
        }

        private KeyValuePair<string, object> GenerateField(string fieldName, string fieldType)
        {
            var field = new KeyValuePair<string, object>();
            FieldType type;

            if (!Enum.TryParse(fieldType, out type))
                throw new ArgumentException($"The field type {fieldType} is not supported !");

            switch (type)
            {
                case FieldType.String:
                    field = new KeyValuePair<string, object>(fieldName, GenerateString());
                    break;
                case FieldType.Integer:
                    field = new KeyValuePair<string, object>(fieldName, GenerateInteger());
                    break;
                case FieldType.Float:
                    field = new KeyValuePair<string, object>(fieldName, GenerateFloat());
                    break;
                case FieldType.Date:
                    field = new KeyValuePair<string, object>(fieldName, GenerateDate());
                    break;
            }

            return field;
        }
 
        public IList<Document> GenerateDocuments(int number = 1)
        {
            var documentList = new List<Document>();
            for (int i = 0; i < number; i++)
            {
                documentList.Add(GenerateDocument());
            }
            return documentList;
        }

       
    }

    class ValueGenerator
    {
        private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = random = new Random();

        public static string GenerateString(int length = 10)
        {
            var generatedString = new char[length];

            for (int i = 0; i < generatedString.Length; i++)
            {
                generatedString[i] = chars[random.Next(chars.Length)];
            }

            return new string(generatedString);
        }

        public static int GenerateInteger(int min = 0, int max = 256)
        {
            return random.Next(min, max);
        }

        public static float GenerateFloat(int min = 0, int max = 256)
        {
            return (float)(random.NextDouble() * max + min);
        }

        public static DateTime GenerateDate()
        {
            var date = DateTime.UtcNow.AddDays(-1 * random.Next(0, 250));
            return date;
        }
    }
}
