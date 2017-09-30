using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Startup
{
    public static class SqlServerConnectionTest
    {
        public static void ConnectionTest()
        {
            string connectionString = @"Server = DESKTOP-2ARV8QK\DINOSTARK; Integrated Security = True; Database = Startup";
            SqlConnection conn = new SqlConnection(connectionString);

            string query = @"Select * From Student";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    Console.WriteLine(reader[0] + " " + reader[1]);

                reader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}