﻿using ChatRoom_project.logics;
using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom_project.Public_Interfaces
{
    /*
     * Handles user table
     */ 
    public class UserHandler: Handler<IUser>
    {
        /*
         * Enables to safely map the enum values to the string representation of the field in the table
         */ 
        enum Fields { Id, Group_Id, Nickname, Password};
        private static readonly Dictionary<Fields, string> fieldsDic = new Dictionary<Fields, string>()
        {
            {Fields.Id, "Id"},
            {Fields.Group_Id, "Group_Id"},
            {Fields.Nickname, "Nickname"},
            {Fields.Password, "Password"}
        };

        private static readonly Fields[] tableColumns =
        {
            Fields.Id,
            Fields.Group_Id,
            Fields.Nickname,
            Fields.Password
        };
        protected override IUser addRow(SqlDataReader data_reader)
        {


            return new HandlerUser(
                        
                        data_reader.GetValue(2).ToString().TrimEnd(),
                        data_reader.GetValue(3).ToString(),
                        (int)data_reader.GetValue(0),
                        (int)data_reader.GetValue(1)

                        );
                        
        }

       
        protected override SqlCommand createSelectCommand(int numOfRows, Dictionary<string, string> query)
        {
            SqlCommand ans = new SqlCommand(null, null);
            string commandString =
                "SELECT U.Id, U.Group_Id, U.Nickname, U.Password" +
                " FROM USERS AS U";
            commandString += " WHERE 1=1";
            if (query.ContainsKey(fieldsDic[Fields.Id]))
                commandString += $"AND U.Id = @USERID";
            if (query.ContainsKey(fieldsDic[Fields.Nickname]))
            {
                commandString += $" AND U.Nickname = @NICKNAME";
            }
            if (query.ContainsKey(fieldsDic[Fields.Group_Id]))
            {
                commandString += $" AND U.Group_Id = @GID";
            }
            ans.CommandText = commandString;

            if (query.ContainsKey(fieldsDic[Fields.Id]))
            {
                ans.Parameters.Add("@USERID", SqlDbType.Int);
                ans.Parameters["@USERID"].Value = Convert.ToInt32(query[fieldsDic[Fields.Id]]);
            }
            if (query.ContainsKey(fieldsDic[Fields.Nickname]))
            {
                ans.Parameters.Add("@NICKNAME", SqlDbType.Char,10);
                ans.Parameters["@NICKNAME"].Value = query[fieldsDic[Fields.Nickname]];
            }
            if (query.ContainsKey(fieldsDic[Fields.Group_Id]))
            {
                ans.Parameters.Add("@GID", SqlDbType.Int);
                ans.Parameters["@GID"].Value = Convert.ToInt32(query[fieldsDic[Fields.Group_Id]]);
            }

            return ans;
        }
        
        protected override SqlCommand createInsertCommand(Dictionary<string, string> query)
        {
            SqlCommand ans = new SqlCommand(null, null);
            string commandString = "INSERT INTO Users(Group_Id, Nickname,Password) " +
                "VALUES(@GID , @NICKNAME , @PASSWORD)";
            ans.CommandText = commandString;
            if (!query.ContainsKey(fieldsDic[Fields.Group_Id]) | !query.ContainsKey(fieldsDic[Fields.Nickname]) | !query.ContainsKey(fieldsDic[Fields.Group_Id])) {
                
                throw new ArgumentException("insert user query Dictionary must contain GID,NICKNAME,PASSWORD query = "+query);
            }
            ans.Parameters.Add("@GID", SqlDbType.Int);
            ans.Parameters["@GID"].Value = Convert.ToInt32(query[fieldsDic[Fields.Group_Id]]);
            ans.Parameters.Add("@NICKNAME", SqlDbType.Char,8);
            ans.Parameters["@NICKNAME"].Value = query[fieldsDic[Fields.Nickname]];
            ans.Parameters.Add("@PASSWORD", SqlDbType.Char,64);
            ans.Parameters["@PASSWORD"].Value = query[fieldsDic[Fields.Password]];




            return ans;
        }
        /*
         * Convert the desired parameters to a dictionary element compatible with the handler 
         */ 
        public Dictionary<string, string> convertToDictionary(string nickname, int g_id, string hashedPassword,int id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(nickname))
            {
                dic[fieldsDic[Fields.Nickname]] =   nickname ;
            }
            if (g_id>0)
            {
                dic[fieldsDic[Fields.Group_Id]] = g_id.ToString();
            }
            if (!string.IsNullOrEmpty(hashedPassword))
            {
                dic[fieldsDic[Fields.Password]] =  hashedPassword ;
            }
            if (id > 0)
            {
                dic[fieldsDic[Fields.Id]] = id.ToString();
            }
            
            return dic;
        }

        
        protected override string createDeleteQuery(Dictionary<string, string> query)
        {
            string ans =
                "DELETE From USERS WHERE 1=1";
            if (query.ContainsKey(fieldsDic[Fields.Id]))
                ans += $"AND Id = {query[fieldsDic[Fields.Id]]}";
            if (query.ContainsKey(fieldsDic[Fields.Nickname]))
            {
                ans += $" AND Nickname = {query[fieldsDic[Fields.Nickname]]}";
            }
            if (query.ContainsKey(fieldsDic[Fields.Group_Id]))
            {
                ans += $" AND Group_Id = {query[fieldsDic[Fields.Group_Id]]}";
            }

            return ans;
        }

        
        private int countRelevantFields(Dictionary<string, string> query)
        {
            int ans = 0;
            foreach (Fields field in tableColumns)
            {
                if (query.ContainsKey(fieldsDic[field]))
                {
                    ans++;
                }
            }
            return ans;
        }

        #region Private Class 

        /// <summary>
        /// class that represent the handler user object
        /// </summary>

        private sealed class HandlerUser : IUser
        {
            private string nickname;
            public string Nickname { get => nickname; }
            private string hashedPassword;
            public string HashedPassword { get => hashedPassword; }
            private int id;
            public int Id { get => id; }
            private int g_id;
            public int G_id { get => g_id; }

            public HandlerUser(string nickname, string hashedPassword, int id, int g_id)
            {
                this.nickname = nickname;
                this.hashedPassword = hashedPassword;
                this.id = id;
                this.g_id = g_id;
            }
        }

        #endregion
    }

}
