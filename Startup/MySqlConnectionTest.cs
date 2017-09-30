using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace Startup
{
    public static class MySqlConnectionTest
    {
        public static void ConnectionTest()
        {
            string connectionString = @"Server = localhost; User Id = root; Password = non-feeling; Database = Startup;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            string query = @"Select * From Student;";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    Console.WriteLine(reader[0] + " " + reader[1]);

                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error Code:" + ex.ErrorCode);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}