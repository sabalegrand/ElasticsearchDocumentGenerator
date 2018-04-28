namespace ElasticsearchStuff
{
    public class Index
    {
        private readonly string value;

        public Index(string value)
        {
            this.value = value;
        }

        public string Value { get { return value; } }
    }
}