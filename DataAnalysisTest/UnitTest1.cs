using DataAnalysis.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DataAnalysisTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            AddConsumers();
            AddSalesman();
            AddSale();
        }

        [Test]
        public void AddConsumers()
        {
            Consumer modelConsumer = new Consumer("�");
            // List<string> listValues = null;
            List<string> listValues = new List<string>(){ "002�2345675434544345�Jose da Silva�Rural" };
            var listConsumer = new List<Consumer>();

            modelConsumer.PopulateConsumer(listValues, ref listConsumer);
            Assert.Pass();
        }

        [Test]
        public void AddSalesman()
        {
            Salesman modelSalesman = new Salesman("�");
            // List<string> listValues = null;
            List<string> listValues = new List<string>() { "001�1234567891234�Pedro�50000" };
            var listSalesman = new List<Salesman>();

            modelSalesman.PopulateSalesman(listValues, ref listSalesman);
            Assert.Pass();
        }

        [Test]
        public void AddSale()
        {
            Sale modelSale = new Sale("�");
            // List<string> listValues = null;
            List<string> listValues = new List<string>() { "003�08�[1-34-10,2-33-1.50,3-40-0.10]�Paulo" };
            var listSale = new List<Sale>();

            var fields = listValues[0].ToString().Split("�");
            modelSale.PopulateSale(listValues, ref listSale);
            SaleItem.PopulateSaleItem(fields[2], 0, ref listSale);
            Assert.Pass();
        }
    }
}