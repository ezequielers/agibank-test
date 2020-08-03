using System;
using System.IO;
using System.Configuration;
using DataAnalysis.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataAnalysis
{
    class Program
    {
        private static string separatorFields = ConfigurationManager.AppSettings["FieldSeparator"];
        static void Main(string[] args)
        {
            //Check required parameters
            if (CheckRequiredParameters())
            {
                // Searches all files .dat and process them
                string pathIn = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["FolderIn"]);

                if (Directory.Exists(pathIn))
                {
                    var dir = new DirectoryInfo(pathIn);

                    var files = dir.GetFiles("*dat", SearchOption.AllDirectories);

                    // Collections files data
                    List<Consumer> consumers = new List<Consumer>();
                    List<Salesman> salesman = new List<Salesman>();
                    List<Sale> sales = new List<Sale>();

                    Salesman modelSalesman = new Salesman(ConfigurationManager.AppSettings["FieldSeparator"]);
                    Consumer modelConsumer = new Consumer(ConfigurationManager.AppSettings["FieldSeparator"]);
                    Sale modelSale = new Sale(ConfigurationManager.AppSettings["FieldSeparator"]);

                    foreach (var file in files)
                    {
                        try
                        {
                            var fileContent = File.ReadAllText(file.FullName);
                            var lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                            if (lines.Length > 0)
                            {
                                var linesSalesmans = lines.Where(x => x.Substring(0, 3) == "001");
                                modelSalesman.PopulateSalesman(linesSalesmans, ref salesman);

                                var linesConsumers = lines.Where(x => x.Substring(0, 3) == "002");
                                modelConsumer.PopulateConsumer(linesConsumers, ref consumers);

                                var linesSales = lines.Where(x => x.Substring(0, 3) == "003");
                                modelSale.PopulateSale(linesSales, ref sales);

                                CreateFileResult(file.Name.Replace(file.Extension, ""), consumers, salesman, sales);

                                // Move file to processed path
                                if (!Directory.Exists(pathIn + "\\processed\\"))
                                {
                                    Directory.CreateDirectory(pathIn + "\\processed\\");
                                }
                                file.MoveTo(pathIn + "\\processed\\" + file.Name);
                            }
                        } catch (Exception error)
                        {
                            // Move file to error path
                            if (!Directory.Exists(pathIn + "\\error\\"))
                            {
                                Directory.CreateDirectory(pathIn + "\\error\\");
                            }
                            file.MoveTo(pathIn + "\\error\\" + file.Name);

                            ErrorWrite();
                        }
                    }
                }
            } else
            {
                ErrorWrite();
            }
        }

        private static void ErrorWrite()
        {
            // Create "data" folder into HOMEPATH
            var homePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%" + "\\data\\logs\\");
            if (!Directory.Exists(homePath))
            {
                Directory.CreateDirectory(homePath);
            }
            var swLog = File.CreateText(String.Format(homePath + "log_{0}.log", DateTime.Now.ToString("yyyymmdd")));
            swLog.WriteLine("The system not encontered all config parameter, please check App.config.");
            swLog.Close();
        }

        private static bool CheckRequiredParameters ()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("FolderIn")
                && ConfigurationManager.AppSettings.AllKeys.Contains("FolderOut")
                && ConfigurationManager.AppSettings.AllKeys.Contains("FieldSeparator"))
                return true;
            else
                return false;
        }

        private static void CreateFileResult (string fileName, List<Consumer> listConsumer, List<Salesman> listSalesman, List<Sale> listSale)
        {
            var pathOut = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["FolderOut"]);

            if (!Directory.Exists(pathOut))
            {
                Directory.CreateDirectory(pathOut);
            }

            var fileOut = File.CreateText(pathOut + fileName.Replace(" ", "_").Replace(".", "_") + ".done.dat");

            fileOut.WriteLine(String.Format("Quantity of clients in file: {0}", listConsumer.Count));

            var totalSalesmans = listSalesman.GroupBy(salesman => salesman.Cpf).Count();
            fileOut.WriteLine(String.Format("Quantity of Salesmans in file: {0}", totalSalesmans));

            var totalSales = listSale.Select(sale => new
            {
                Id = sale.SaleId,
                Price = sale.Items.Sum(saleItem => (saleItem.Quantity * saleItem.Price)),
                sale.SalesmanName
            });
            var biggestSale = totalSales.OrderByDescending(item => item.Price).First();

            fileOut.WriteLine(String.Format("Biggest sale Id: {0}", biggestSale.Id));

            fileOut.Close();
        }
    }
}
