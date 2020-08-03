using System;
using System.Collections.Generic;
using System.Configuration;

namespace DataAnalysis.Models
{
    public class Salesman
    {
        public string Cpf { get; set; }

        public string Name { get; set; }

        public double Salary { get; set; }

        private string _separatorFields { get; set; }

        public Salesman (string separatorFields)
        {
            _separatorFields = separatorFields;
        }

        /// <summary>
        /// Papulate Salesmans into the collection variable
        /// </summary>
        /// <param name="listSalesmanLines">List of lines finded into the file</param>
        /// <param name="listSalesman">Collection of Salesmans passed by reference</param>
        public void PopulateSalesman(IEnumerable<string> listSalesmanLines, ref List<Salesman> listSalesman)
        {
            
            foreach (var item in listSalesmanLines)
            {
                var fields = item.Split(_separatorFields);

                listSalesman.Add(new Salesman(_separatorFields)
                {
                    Cpf = fields[1],
                    Name = fields[2],
                    Salary = Convert.ToDouble(fields[3].Replace(".", ","))
                });
            }
        }
    }
}
