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
    public class MessageHandler:Handler<IMessage>
    {
        enum Fields{ Guid, SendTime, User_Id, Nickname, Group_Id, Body};
        private static readonly Dictionary<Fields, string> fieldsDic = new Dictionary<Fields, string>()
        {
            {Fields.Guid, "Guid"},
            {Fields.SendTime, "SendTime"},
            {Fields.User_Id, "User_Id"},
            {Fields.Nickname, "Nickname"},
            {Fields.Group_Id, "Group_Id"},
            {Fields.Body, "Body"}
        };
        private static readonly Fields[] tableColumns = 
        {
            Fields.Guid,
            Fields.User_Id,
            Fields.SendTime,
            Fields.Body
        };

        protected override IMessage addRow(SqlDataReader data_reader)
        {
            DateTime dateFacturation = new DateTime();
            if (!data_reader.IsDBNull(1))
                dateFacturation = data_reader.GetDateTime(1);

            return new Message(
                        new Guid(),
                        data_reader.GetValue(0).ToString(),
                        dateFacturation,
                        data_reader.GetValue(2).ToString(),
                        data_reader.GetValue(3).ToString()
                        );
        }

        protected override string createSelectQuery(int numOfRows, Dictionary<string, string> query)
        {
            string ans = "SELECT";
            if (numOfRows>0) {
                ans += $" TOP {numOfRows}";
            }
            ans+=" U.Nickname, M.SendTime, M.Body, U.Group_Id" +
                " FROM Messages AS M JOIN USERS AS U ON M.User_Id =U.Id";
            ans += " WHERE 1=1";
            if (query.ContainsKey(fieldsDic[Fields.Guid]))
            {
                ans += $" AND Guid = {query[fieldsDic[Fields.Guid]]}";
            }
            if (query.ContainsKey(fieldsDic[Fields.SendTime]))
            {
                ans += $" AND SendTime >= {query[fieldsDic[Fields.SendTime]]}";
            }
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
            string ans = "INSERT INTO Messages(";
            int size = countRelevantFields(query);
            int i = 0;
            foreach (Fields field in tableColumns) {
                if (query.ContainsKey(fieldsDic[field])) {
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
        protected override string createDeleteQuery(Dictionary<string, string> query)
        {
            string ans = "DELETE M FROM" +
                " Messages AS M JOIN USERS AS U ON M.User_Id =U.Id";
            ans += " WHERE 1=1";
            if (query.ContainsKey(fieldsDic[Fields.Guid]))
            {
                ans += $" AND Guid = {query[fieldsDic[Fields.Guid]]}";
            }
            if (query.ContainsKey(fieldsDic[Fields.SendTime]))
            {
                ans += $" AND SendTime > {query[fieldsDic[Fields.SendTime]]}";
            }
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
        public Dictionary<string, string> convertToDictionary(Guid guid,DateTime date, int userId, string nickname, int g_Id, string body)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!guid.Equals(default(Guid))) {
                dic[fieldsDic[Fields.Guid]] = "'"+guid.ToString()+"'";
            }
            if (date.CompareTo(DateTime.MinValue) > 0)
            { 
                dic[fieldsDic[Fields.SendTime]] = "'"+date.ToString()+ "'";
            }
            if (userId != 0)
            {
                dic[fieldsDic[Fields.User_Id]] = userId.ToString();
            }
            if (nickname != null)
            {
                dic[fieldsDic[Fields.Nickname]] = "'"+nickname+"'";
            }
            if (g_Id > 0)
            {
                dic[fieldsDic[Fields.Group_Id]] = g_Id.ToString();
            }
            if (body != null)
            {
                dic[fieldsDic[Fields.Body]] = "'"+body+"'";
            }
            return dic;
        }
        public Dictionary<string, string> convertToDictionary(IMessage msg, int userId)
        {
            int g_id;
            int.TryParse(msg.GroupID, out g_id);
            return convertToDictionary(
                msg.Id,
                msg.Date, //problematic. better avoid
                userId,
                msg.UserName,
                g_id,
                msg.MessageContent
                );
        }
        /*
         * Counts the amount of fields in query that are in the handler table
         */
        private int countRelevantFields(Dictionary<string, string> query)
        {
            int ans = 0;
            foreach (Fields field in tableColumns) {
                if (query.ContainsKey(fieldsDic[field])) {
                    ans++;
                }
            }
            return ans;
        }


    }
}
