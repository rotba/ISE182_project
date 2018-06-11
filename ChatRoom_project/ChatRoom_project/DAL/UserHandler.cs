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
    public class UserHandler: Handler<IUser>
    {
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


            return new User(
                        (int)data_reader.GetValue(0),
                        (int)data_reader.GetValue(1),
                        data_reader.GetValue(2).ToString()
                        );
                        
        }

        protected override string createSelectQuery(int numOfRows, Dictionary<string, string> query)
        {
            string ans =
                "SELECT U.Id, U.Group_Id, U.Nickname, U.Password" +
                " FROM USERS AS U";
            ans += " WHERE 1=1";
            if (query.ContainsKey(fieldsDic[Fields.Id]))
                ans += $"AND U.Id = {query[fieldsDic[Fields.Id]]}";
            if (query.ContainsKey(fieldsDic[Fields.Nickname]))
            {
                ans += $" AND U.Nickname = {query[fieldsDic[Fields.Nickname]]}";
            }
            if (query.ContainsKey(fieldsDic[Fields.Group_Id]))
            {
                ans += $" AND U.Group_Id = {query[fieldsDic[Fields.Group_Id]]}";
            }
            
            return ans;
        }

        protected override string createInsertQuery(Dictionary<string, string> query)
        {
            string ans = "INSERT INTO Users(";
            int size = countRelevantFields(query);
            int i = 0;
            foreach (Fields field in tableColumns)
            {
                if (query.ContainsKey(fieldsDic[field]))
                {
                    ans += fieldsDic[field];
                    if (i != size - 1)
                    {
                        ans += ", ";
                        i++;
                    }
                }
            }
            ans += ")";
            ans += " VALUES(";
            i = 0;
            foreach (Fields field in tableColumns)
            {
                if (query.ContainsKey(fieldsDic[field]))
                {
                    ans += query[fieldsDic[field]];
                    if (i != size - 1)
                    {
                        ans += ", ";
                        i++;
                    }
                }
            }
            ans += ")";

            return ans;
        }

        internal Dictionary<string, string> convertToDictionary(string nickname, int g_id, string hashedPassword,int id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(nickname))
            {
                dic[fieldsDic[Fields.Nickname]] = "'" + nickname + "'";
            }
            if (g_id>0)
            {
                dic[fieldsDic[Fields.Group_Id]] = g_id.ToString();
            }
            if (!string.IsNullOrEmpty(hashedPassword))
            {
                dic[fieldsDic[Fields.Password]] = "'" + hashedPassword + "'";
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

        protected override SqlCommand createSelectQuery(int numOfRows, Dictionary<string, string> query, bool test)
        {
            string ans =
                "SELECT U.Id, U.Group_Id, U.Nickname, U.Password" +
                " FROM USERS AS U";
            ans += " WHERE 1=1";

            if (query.ContainsKey(fieldsDic[Fields.Nickname]))
            {
                ans += $" AND U.Nickname = {query[fieldsDic[Fields.Nickname]]}";
            }
            if (query.ContainsKey(fieldsDic[Fields.Group_Id]))
            {
                ans += $" AND U.Group_Id = {query[fieldsDic[Fields.Group_Id]]}";
            }
            ans += " ORDER BY SendTime";
            return null;
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
    }
}
