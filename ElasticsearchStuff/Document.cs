using System.Collections.Generic;

namespace ElasticsearchStuff
{
    public class Document
    {
 
        private IDictionary<string, object> fields;

        public IDictionary<string, object> Fields
        {
            get
            {
                return fields;
            }
        }

        public Document()
        {
            fields = new Dictionary<string, object>();
        }

        public Document(IDictionary<string, object> fields)
        {
            this.fields = fields;
        }


        public void AddField(string key, object value)
        {
            fields.Add(key, value);
        }
        public void AddField(KeyValuePair<string, object> field)
        {
            fields.Add(field.Key, field.Value);
        }

        public IDictionary<string, object> AsDictionary()
        {
            return fields;
        }

    }
}