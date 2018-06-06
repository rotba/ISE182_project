﻿using ChatRoom_project.logics;
using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom_project.DAL
{
    public class UserHandler
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
        public bool insert(User user)
        {
            return false;
        }
        public List<User> retrieve(int number,string nickname, int g_ID)
        {
            List<User> ans = new List<User>();
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
                sql_query = createQuery(number, nickname, g_ID);
                command = new SqlCommand(sql_query, connection);
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    int curr_gid =-1;
                    int curr_id=-1;
                    if (!data_reader.IsDBNull(1))
                    {
                        int.TryParse(data_reader.GetValue(0).ToString(), out curr_id);
                        int.TryParse(data_reader.GetValue(1).ToString(), out curr_gid);
                    }
                    ans.Add(new User(
                        curr_id,
                        curr_gid,
                        data_reader.GetValue(2).ToString()
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

        private string createQuery(int number, string nickname, int g_ID)
        {
            string ans =
                "SELECT U.Id, U.Group_Id, U.Nickname" +
                " FROM USERS AS U";
            ans += " WHERE 1=1";
            
            if (nickname != null)
            {
                ans += $" AND U.Nickname = {nickname}";
            }
            if (g_ID > 0)
            {
                ans += $" AND U.Group_Id = {g_ID}";
            }
            if (number>0) {
                ans += $" LIMIT {number}";
            }
            return ans;
        }
    }
}
