namespace ElasticsearchStuff
{
    public class DocumentType
    {
        private readonly string value;

        public DocumentType(string value)
        {
            this.value = value;
        }

        public string Value { get { return value; } }
    }
}