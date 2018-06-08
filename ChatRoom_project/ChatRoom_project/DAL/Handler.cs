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
        
        //local
        private static readonly string connetion_string = $"Server= localhost\\sqlexpress; Database= MS3; Integrated Security=True;";
        //defualt
        //private static readonly string connetion_string = $"Data Source=ise172.ise.bgu.ac.il,1433\\DB_LAB;Initial Catalog=db_lab;User ID=db_lab_user;Password=hackMePlease";
          
        public T insert(Dictionary<string, string> query)
        {
            T ans = default(T);
            
            using (SqlConnection connection = new SqlConnection(connetion_string))
            {
                connection.Open();
                using (SqlCommand cmd1 = new SqlCommand(createInsertQuery(query), connection))
                {
                    cmd1.ExecuteNonQuery();//Execute insert
                }
                using (SqlCommand cmd2 = new SqlCommand(createSelectQuery(1, query), connection))
                {
                    SqlDataReader data_reader = cmd2.ExecuteReader();//Retrive inserted item
                    while (data_reader.Read())
                    {
                        ans = addRow(data_reader);
                    }
                }
            }
            
            return ans;
        }
        public List<T> retrieve(int numOfRows, Dictionary<string, string> query)
        {
            List<T> ans = new List<T>();
            using (SqlConnection connection = new SqlConnection(connetion_string))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(createSelectQuery(numOfRows, query), connection);
                SqlDataReader data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    ans.Add(addRow(data_reader));
                }
                data_reader.Close();
                command.Dispose();
            }
            return ans;
        }
        public void delete(Dictionary<string, string> query)
        {
            using (SqlConnection connection = new SqlConnection(connetion_string))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(createDeleteQuery(query), connection);
                SqlDataReader data_reader = command.ExecuteReader();
                data_reader.Close();
                command.Dispose();
            }
        }
        protected abstract T addRow(SqlDataReader data_reader);
        protected abstract string createSelectQuery(int numOfRows, Dictionary<string, string> query);
        protected abstract string createInsertQuery(Dictionary<string, string> query);
        protected abstract string createDeleteQuery(Dictionary<string, string> query);


    }
}
