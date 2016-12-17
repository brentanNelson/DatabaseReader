using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseReader
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadDatabase();
        }

        public static void ReadDatabase()
        {
            const string Delimiter = "\"";
            const string Separator = ",";

            var dateString = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            using (var writer = new StreamWriter(ConfigurationManager.AppSettings["OutputFileLocation"] + dateString +"_dump.csv"))
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            using(var command = new SqlCommand(ConfigurationManager.AppSettings["Query"], connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
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
