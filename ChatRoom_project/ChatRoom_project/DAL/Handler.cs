using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom_project.DAL
{
    public abstract class Handler<T>
    {
        /// 
        /*
         *local
         */
        string connetion_string = null;
        string sql_query = null;
        string server_address = "localhost\\sqlexpress";
        string database_name = "MS3";
        string user_name = "";
        string password = "";
        /*
         *default
        string connetion_string = null;
        string sql_query = null;
        string server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
        string database_name = "db_lab";
        string user_name = "db_lab_user";
        string password = "hackMePlease";
         */
        /// 
        public bool insert(T item)
        {
            return false;
        }
        public List<T> retrieve(int numOfRows, Dictionary<string, string> query)
        {
            List<T> ans = new List<T>();
            SqlConnection connection;
            SqlCommand command;
            //defualt
            //connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";

            //local
            connetion_string = $"Server= {server_address}; Database= {database_name}; Integrated Security=True;";

            connection = new SqlConnection(connetion_string);
            SqlDataReader data_reader;

            try
            {
                connection.Open();
                Console.WriteLine("connected to: " + server_address);
                sql_query = createQuery(numOfRows, query);
                command = new SqlCommand(sql_query, connection);
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    ans.Add(addRow(data_reader));
                }
                data_reader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            return ans;
        }

        protected abstract T addRow(SqlDataReader data_reader);
        protected abstract string createQuery(int numOfRows, Dictionary<string, string> query);
        
    }
}
