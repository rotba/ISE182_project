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
    public class UserHandler: Handler<User>
    {
        enum Fields { Id, Group_Id, Nickname, Password};
        private static readonly Dictionary<Fields, string> fieldsDic = new Dictionary<Fields, string>()
        {
            {Fields.Id, "Id"},
            {Fields.Group_Id, "Group_Id"},
            {Fields.Nickname, "Nickname"},
            {Fields.Password, "Password"}
        };
        protected override User addRow(SqlDataReader data_reader)
        {
            throw new NotImplementedException();
        }

        protected override string createSelectQuery(int numOfRows, Dictionary<string, string> query)
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
            return ans;
        }

        protected override string createInsertQuery(Dictionary<string, string> query)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
