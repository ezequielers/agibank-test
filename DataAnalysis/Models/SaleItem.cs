using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataAnalysis.Models
{
    public class SaleItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
        
        /// <summary>
        /// Papulate Sales Items into the collection variable
        /// </summary>
        /// <param name="lineSaleItem">List of lines finded into the file</param>
        /// <param name="indexItem">Index of Sale to add Items Sales</param>
        /// <param name="listSale">Collection of Sales passed by reference</param>
        public static void PopulateSaleItem(string lineSaleItem, int indexItem, ref List<Sale> listSale)
        {
            Regex regex = new Regex(@"\[(.*?)\]");
            MatchCollection matches = regex.Matches(lineSaleItem);

            foreach (Match match in matches)
            {
                var listSaleItemsLines = match.Groups[1].Value.Split(",");

                foreach (var item in listSaleItemsLines)
                {
                    var saleItemLine = item.Split("-");
                    listSale[indexItem].Items.Add(new SaleItem()
                    {
                        Id = Convert.ToInt32(saleItemLine[0]),
                        Quantity = Convert.ToInt32(saleItemLine[1]),
                        Price = Convert.ToDouble(saleItemLine[2].Replace(".", ","))
                    });
                }
            }
        }
    }
}
