using System;
using System.Collections.Generic;
using System.Configuration;

namespace DataAnalysis.Models
{
    public class Sale
    {
        public int SaleId { get; set; }

        public List<SaleItem> Items { get; set; }

        public string SalesmanName { get; set; }

        private string _separatorFields { get; set; }
        
        public Sale (string separatorFields)
        {
            Items = new List<SaleItem>();
            _separatorFields = separatorFields;
        }

        /// <summary>
        /// Papulate Sales into the collection variable
        /// </summary>
        /// <param name="listSaleLines">List of lines finded into the file</param>
        /// <param name="listSale">Collection of Sales passed by reference</param>
        public void PopulateSale(IEnumerable<string> listSaleLines, ref List<Sale> listSale)
        {
            foreach (var item in listSaleLines)
            {
                var fields = item.Split(_separatorFields);

                listSale.Add(new Sale(_separatorFields)
                {
                    SaleId = Convert.ToInt32(fields[1]),
                    SalesmanName = fields[3]
                });
                SaleItem.PopulateSaleItem(fields[2], (listSale.Count - 1), ref listSale);
            }
        }
    }
}
