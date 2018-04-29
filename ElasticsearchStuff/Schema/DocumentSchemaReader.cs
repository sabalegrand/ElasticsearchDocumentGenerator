using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace ElasticsearchStuff.Schema
{
    class DocumentSchemaReader
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        private readonly string schemaPath;

        public DocumentSchemaReader(string schemaPath)
        {
            if (!File.Exists(schemaPath))
                throw new ArgumentException("The file path is incorrecte.");
            this.schemaPath = schemaPath;
        }
 
        public List<DocumentField> FieldSchema { get; set; }


        #region Private Methods
        private JToken GetJsonSchema()
        {
            JToken fieldSchemaList;
            StreamReader file = null;
            try
            {
                file = File.OpenText(schemaPath);
                JsonSerializer serializer = new JsonSerializer();
                var jsonSchema = (JObject)serializer.Deserialize(file, typeof(JObject));
                fieldSchemaList = jsonSchema.SelectToken("DocumentSchema");
            }
            catch (Exception)
            {
                logger.Error("An error has occured during the schema deserialization.");
                throw;
            }
            finally
            {
                if (file != null)
                    file.Close();
            }

            return fieldSchemaList;
        }

        private DocumentField GetField(JToken fieldSchema)
        {
            DocumentField field = null;
            FieldType fieldType;

            var jsonFieldType = GetValueFromToken(fieldSchema,DocumentField.TYPE).ToString();
            if (!Enum.TryParse(jsonFieldType, out fieldType))
                throw new ArgumentException($"The field type {jsonFieldType} is not supported !");

            switch (fieldType)
            {
                case FieldType.String:
                    field = new StringField(
                            GetValueFromToken(fieldSchema, DocumentField.FIELD_NAME).ToString(),
                            fieldType,
                            GetValueFromToken(fieldSchema, StringField.LENGTH).ToObject<int>(),
                            GetValueFromToken(fieldSchema, StringField.POSSIBLE_VALUES).ToObject<List<string>>()
                        );
                    break;
                case FieldType.Integer:
                    field = new IntegerField(
                             GetValueFromToken(fieldSchema, DocumentField.FIELD_NAME).ToString(),
                             fieldType,
                             GetValueFromToken(fieldSchema, IntegerField.MIN).ToObject<int>(),
                             GetValueFromToken(fieldSchema, IntegerField.MAX).ToObject<int>()
                        );
                    break;
                case FieldType.Float:
                    field = new FloatField(
                             GetValueFromToken(fieldSchema, DocumentField.FIELD_NAME).ToString(),
                             fieldType,
                             GetValueFromToken(fieldSchema, FloatField.MIN).ToObject<float>(),
                             GetValueFromToken(fieldSchema, FloatField.MAX).ToObject<float>()
                        );
                    break;
                case FieldType.Date:
                    field = new DateField(
                             GetValueFromToken(fieldSchema, DocumentField.FIELD_NAME).ToString(),
                             fieldType,
                             GetValueFromToken(fieldSchema, DateField.LOWER).ToObject<DateTime>(),
                             GetValueFromToken(fieldSchema, DateField.UPPER).ToObject<DateTime>()
                        );
                    break;
            }

            return field;
        }

        private JToken GetValueFromToken(JToken fieldSchema, string fieldName)
        {
            var value = fieldSchema.SelectToken(fieldName);
            if (value != null)
                return value;

            throw new ArgumentException($"Invalid schema: the token '{fieldName}' cannot be found.");
        }
        #endregion

        public List<DocumentField> GetDocumentSchema()
        {
            var documentFields = new List<DocumentField>();

            var fieldSchemaList = GetJsonSchema();

            try
            {
                foreach (var fieldSchema in fieldSchemaList)
                {
                    documentFields.Add(GetField(fieldSchema));
                }
            }
            catch(Exception)
            {
                logger.Error("Failed to retrieve field schema. Make sure that a correcte syntax is used.");
                throw;
            }
            
            return documentFields;
        }
       

       
    }
}
