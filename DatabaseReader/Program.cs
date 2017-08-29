using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ReadDatabase();
                Console.WriteLine("Successfully Complete!");
            }
            catch (Exception e)
            {
                Console.WriteLine("An Error has occured! " + e.Message + " /r/n " + e.StackTrace);
                Console.Read();
            }
        }

        public static void ReadDatabase()
        {
            const string Delimiter = "\"";
            const string Separator = ",";

            Console.WriteLine("Setting up program");

            using (var writer = new StreamWriter(ConfigurationManager.AppSettings["OutputFileLocation"]))
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            using(var command = new MySqlCommand(ConfigurationManager.AppSettings["Query"], connection))
            {
                Console.WriteLine("Opening connection");

                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("writing headers");

                    // write header row
                    for (int columnCounter = 0; columnCounter < reader.FieldCount; columnCounter++)
                    {
                        if (columnCounter > 0)
                        {
                            writer.Write(Separator);
                        }
                        writer.Write(Delimiter + reader.GetName(columnCounter) + Delimiter);
                    }
                    writer.WriteLine(string.Empty);

                    Console.WriteLine("populating data");

                    while (reader.Read())
                    {
                        // column loop
                        for (int columnCounter = 0; columnCounter < reader.FieldCount; columnCounter++)
                        {
                            if (columnCounter > 0)
                            {
                                writer.Write(Separator);
                            }
                            writer.Write(Delimiter + reader.GetValue(columnCounter).ToString().Replace('"', '\'') + Delimiter);
                        }   
                        writer.WriteLine(string.Empty);
                    }   // data loop
                    writer.Flush();
                }
            }
        }
    }
}
