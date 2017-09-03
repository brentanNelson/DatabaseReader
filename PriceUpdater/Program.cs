using CsvHelper;
using MySql.Data.MySqlClient;
using PriceUpdater.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var csvData = ReadCsv();
                var update = UpdatePrices(csvData);
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                Console.Read();
            }
        }

        static List<UpdatedPriceData> ReadCsv()
        {
            Console.WriteLine($"Reading data from csv..");
            var csvData = new List<UpdatedPriceData>();
            using (var reader = new StreamReader(ConfigurationManager.AppSettings["InputFileLocation"]))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap(new UpdatedPriceDataMap());

                csvData.AddRange(csv.GetRecords<UpdatedPriceData>().ToList());
            }
            Console.WriteLine($"{csvData.Count()} records found.");
            return csvData;
        }

        static bool UpdatePrices(List<UpdatedPriceData> newPriceData)
        {
            Console.WriteLine($"Updating database");
            var totalUpdated = 0;
            foreach (var priceData in newPriceData)
            {
                double parseResult;
                var isADouble  = Double.TryParse(priceData.CostP1, out parseResult);
                Console.WriteLine($"Attempting to set {priceData.Product} to ${priceData.CostP1}");
                if (string.IsNullOrWhiteSpace(priceData.Product) || !isADouble)
                {
                    Console.Error.WriteLine($"Error: Invalid data in {priceData.Product} or {priceData.CostP1}, product must not be empty and costp1 should be a double value.");
                    break;
                }
                
                var query = $@"UPDATE prodsite
                           SET COSTP1 = {priceData.CostP1}
                           WHERE PRODUCT = '{priceData.Product}';";

                using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    Console.WriteLine("Opening connection");
                    try
                    {
                        connection.Open();
                        var updatedRows = command.ExecuteNonQuery();
                        totalUpdated += updatedRows;
                        Console.WriteLine($"Successfully updated {updatedRows} rows.");
                    }
                    catch (MySqlException exSql)
                    {
                        Console.Error.WriteLine("Error - SQL Exception: " + exSql);
                        Console.Error.WriteLine(exSql.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error - Exception: " + ex);
                        Console.Error.WriteLine(ex.StackTrace);
                    }
                }
            }

            Console.WriteLine($"Successfully updated {totalUpdated} rows in total.");
            return true;
        }
    }
}
