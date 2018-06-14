using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom_project.Public_Interfaces
{
    /*
     * Represents auxilery unit that enables data accessing and insertion to a table
     */ 
    public abstract class Handler<T>
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //local
       //private static readonly string connetion_string = $"Server= localhost\\sqlexpress; Database= MS3; Integrated Security=True;";
        //defualt
        private static readonly string connetion_string = $"Data Source=ise172.ise.bgu.ac.il,1433\\DB_LAB;Initial Catalog=MS3;User ID=publicUser;Password=isANerd";
        
        /*
         * Inserts row to the table according to the query specification
         */
        public T insert(Dictionary<string, string> query)
        {
            T ans = default(T);

            using (SqlConnection connection = new SqlConnection(connetion_string))
            {
                connection.Open();
                SqlCommand cmd1 = createInsertCommand(query);
                cmd1.Connection = connection;
                using ( cmd1 )
                {
                    cmd1.ExecuteNonQuery();//Execute insert
                }
                SqlCommand cmd2 = createSelectCommand(1, query);
                cmd2.Connection = connection;
                using (cmd2)
                {
                    SqlDataReader data_reader = cmd2.ExecuteReader();//Retrive inserted item
                    while (data_reader.Read())
                    {
                        try
                        {
                            ans = addRow(data_reader);
                        }
                        catch (ArgumentException e)
                        {
                            log.Debug("DB INVALID ROW" + e);
                        }
                    }
                }
            }

            return ans;
        }

        /*
         * Rereieves rows from  the table according to the query specification
         */
        public List<T> retrieve(int numOfRows, Dictionary<string, string> query)
        {
            List<T> ans = new List<T>();
            using (SqlConnection connection = new SqlConnection(connetion_string))
            {
                connection.Open();
                SqlCommand command = createSelectCommand(numOfRows, query);
                command.Connection = connection;
                SqlDataReader data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    try
                    {
                        ans.Add(addRow(data_reader));
                    }
                    catch (ArgumentException e)
                    {
                        log.Debug("DB INVALID ROW" + e);
                    }
                }
                data_reader.Close();
                command.Dispose();
            }
            return ans;
        }

        //for tests only
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
        /*
         * Converts the row currently pointed by data_reader to T element
         */ 
        protected abstract T addRow(SqlDataReader data_reader);
        /*
         * Returns SqlCommand for SELECT query with respect to the values specified in the given quey
         */ 
        protected abstract SqlCommand createSelectCommand(int numOfRows, Dictionary<string, string> query);
        /*
         * Returns SqlCommand for INSERT query with respect to the values specified in the given quey
         */
        protected abstract SqlCommand createInsertCommand(Dictionary<string, string> query);
        //**FOR TESTS**//
        protected abstract string createDeleteQuery(Dictionary<string, string> query);
    }
}
