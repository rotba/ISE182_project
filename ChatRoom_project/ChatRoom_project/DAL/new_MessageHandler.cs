using ConsoleApp1.BuissnessLayer;
using MileStoneClient.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom_project.DAL
{
    class new_MessageHandler
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
        public bool insert(IMessage message) {
            return false;
        }
        public List<IMessage> retrieve(DateTime lastRecieved, int number,
            string nickname, int g_ID)
        {
            List<IMessage> ans = new List<IMessage>();
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
                sql_query = createQuery(lastRecieved, number,nickname, g_ID);
                command = new SqlCommand(sql_query, connection);
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    DateTime dateFacturation = new DateTime();
                    if (!data_reader.IsDBNull(1))
                        dateFacturation = data_reader.GetDateTime(1); //2 is the coloumn index of the date. There are such
                    ans.Add(new Message(
                        new Guid(),
                        data_reader.GetValue(0).ToString(),
                        dateFacturation,
                        data_reader.GetValue(3).ToString(),
                        data_reader.GetValue(4).ToString()
                        ));
                    
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

        private string createQuery(DateTime lastRecieved, int number, string nickname, int g_ID)
        {
            string ans =
                "SELECT U.Nickname M.SendTime M.Body, U.Group_Id" +
                " FROM Messages AS M JOIN USERS AS U ON M.User_Id =U.Id";
            ans+=" WHERE 1=1";
            if (lastRecieved>DateTime.MinValue) {
                ans += $" AND SendTime > {lastRecieved.ToString()}";
            }
            if (nickname !=null)
            {
                ans += $" AND U.Nickname = {nickname}";
            }
            if (g_ID > 0)
            {
                ans += $" AND U.Group_Id = {g_ID}";
            }
            ans+= " ORDER BY SendTime";
            if (number>=0) {
                ans += $" LIMIT {number}";
            }
            return ans;
        }

        public void update(IMessage message, string newContent) {
        }
    }
}
