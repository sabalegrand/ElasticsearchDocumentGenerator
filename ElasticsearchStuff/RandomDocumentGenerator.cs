using ElasticsearchStuff.Schema;
using log4net;
using System;
using System.Collections.Generic;
using static ElasticsearchStuff.ValueGenerator;

namespace ElasticsearchStuff
{

  
    class RandomDocumentGenerator
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RandomDocumentGenerator));

        private List<DocumentField> documentSchema;

        public RandomDocumentGenerator(List<DocumentField> documentSchema)
        {
            this.documentSchema = documentSchema;
        }

        public Document GenerateDocument()
        {
            Document document = new Document();
            try
            {
                foreach (var fieldSchema in documentSchema)
                {
                    document.AddField(GenerateField(fieldSchema));
                }
            }
            catch (Exception)
            {
                logger.Error("An error has occurred during the document generation.");
                throw;
            }
         
             
            return document;
        }

        private KeyValuePair<string, object> GenerateField(DocumentField fieldSchema)
        {
            var field = new KeyValuePair<string, object>(); 

            switch (fieldSchema.FieldType)
            {
                case FieldType.String:
                    var stringField = (StringField)fieldSchema;
                    field = new KeyValuePair<string, object>(stringField.FieldName, GenerateString(stringField.Length, stringField.PossibleValues));
                    break;
                case FieldType.Integer:
                    var integerField = (IntegerField)fieldSchema;
                    field = new KeyValuePair<string, object>(integerField.FieldName, GenerateInteger(integerField.Min, integerField.Max));
                    break;
                case FieldType.Float:
                    var floatField = (FloatField)fieldSchema;
                    field = new KeyValuePair<string, object>(floatField.FieldName, GenerateFloat(floatField.Min, floatField.Max));
                    break;
                case FieldType.Date:
                    var dateField = (DateField)fieldSchema;
                    field = new KeyValuePair<string, object>(dateField.FieldName, GenerateDate(dateField.Lower, dateField.Upper));
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

        public static string GenerateString(int length = 10, List<string> possibleValues = null)
        {
            if (possibleValues != null && possibleValues.Count != 0)
                return possibleValues[random.Next(0, possibleValues.Count - 1)];

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

        public static float GenerateFloat(float min = 0, float max = 256)
        {
            return (float)(random.NextDouble() * max + min);
        }

        public static DateTime GenerateDate(DateTime lower, DateTime upper)
        {
            var date = DateTime.UtcNow.AddDays(-1 * random.Next(0, 250));
            return date;
        }
    }
}
