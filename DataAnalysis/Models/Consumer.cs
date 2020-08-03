using System;
using System.Collections.Generic;
using System.Configuration;

namespace DataAnalysis.Models
{
    public class Consumer
    {
        public string Cnpj { get; set; }

        public string Name { get; set; }

        public string BusinessArea { get; set; }

        private string _separatorFields { get; set; }

        public Consumer(string separatorFields)
        {
            _separatorFields = separatorFields;
        }

        /// <summary>
        /// Papulate Consumers into the collection variable
        /// </summary>
        /// <param name="listConsumerLines">List of lines finded into the file</param>
        /// <param name="listConsumer">Collection of Consumers passed by reference</param>
        public void PopulateConsumer(IEnumerable<string> listConsumerLines, ref List<Consumer> listConsumer)
        {
            foreach (var item in listConsumerLines)
            {
                var fields = item.Split(_separatorFields);

                listConsumer.Add(new Consumer (_separatorFields)
                {
                    Cnpj = fields[1],
                    Name = fields[2],
                    BusinessArea = fields[3]
                });
            }
        }
    }
}
