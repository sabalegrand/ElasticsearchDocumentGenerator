using log4net;
using System;
using System.Linq;
using System.Threading;

namespace ElasticsearchStuff
{
    class ContinuousBulkInserter: BulkInserter
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        private readonly RandomDocumentGenerator documentGenerator;
        private readonly Index index;
        private int intervalInSecondes;
        private readonly DocumentType type;

        public ContinuousBulkInserter(
            Uri host,
            RandomDocumentGenerator documentGenerator,
            Index index,
            DocumentType type,
            int intervalInSecondes)
            :base(host)
        {
            this.intervalInSecondes = intervalInSecondes;
            this.documentGenerator = documentGenerator;
            this.index = index;
            this.type = type;
        }

        public void Insert(int numberOfDocuments)
        {
            logger.Info($"Starting periodic bulk insertion with interval of {intervalInSecondes} seconds");

            while (true)
            {
                var listDocument = documentGenerator.GenerateDocuments(numberOfDocuments);
                logger.Info($"Indexing {numberOfDocuments} documents...");
                BulkInsert(listDocument.Select(doc => doc.AsDictionary()).ToList(), index, type);
                
                Thread.Sleep(intervalInSecondes * 1000);
            }
        }
    }
}
