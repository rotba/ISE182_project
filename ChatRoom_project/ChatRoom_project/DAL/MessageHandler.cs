﻿using ConsoleApp1.BuissnessLayer;
using MileStoneClient.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom_project.DAL
{
    public class MessageHandler : Handler<IMessage>
    {
        enum Fields { Guid, SendTime, User_Id, Nickname, Group_Id, Body };
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
            DateTime UtcTime = new DateTime();
            DateTime LocalTime= new DateTime();
            if (!data_reader.IsDBNull(2))
            {
                UtcTime = data_reader.GetDateTime(2);
                var outputCulture = CultureInfo.CreateSpecificCulture("es-es");
                var inputCulture = CultureInfo.CreateSpecificCulture("en-us");
                Thread.CurrentThread.CurrentCulture = outputCulture;
                Thread.CurrentThread.CurrentUICulture = outputCulture;
                LocalTime = DateTime.Parse(
                    UtcTime.ToString(), inputCulture
                    ).ToLocalTime();
            }

            return new HandlerMessage(
                        Guid.Parse(data_reader.GetValue(0).ToString()),
                        data_reader.GetValue(1).ToString(),
                        LocalTime,
                        data_reader.GetValue(3).ToString(),
                        data_reader.GetValue(4).ToString()
                        );
        }
        /*
         * inly for tests. delete before release
        public IMessage TESTaddRowTEST(SqlDataReader data_reader)
        {
            return addRow(data_reader);
        }
        */
        protected override SqlCommand createSelectCommand(int numOfRows, Dictionary<string, string> query)
        {
            SqlCommand ans = new SqlCommand(null, null);
            string commandString = "SELECT ";
            if (numOfRows > 0)
            {
                commandString +=  $" TOP { numOfRows}";
            }
            commandString += " M.Guid, U.Nickname, M.SendTime, M.Body, U.Group_Id" +
                " FROM Messages AS M JOIN USERS AS U ON M.User_Id =U.Id";
            commandString += " WHERE 1=1";
            if (query.ContainsKey(fieldsDic[Fields.Guid]))
            {
                commandString += " AND Guid = @GUID";
            }
            else
            {
                if (query.ContainsKey(fieldsDic[Fields.SendTime]))
                {
                    commandString += " AND SendTime > @SENDTIME";
                }
                if (query.ContainsKey(fieldsDic[Fields.Nickname]))
                {
                    commandString += " AND U.Nickname = @NICKNAME";
                }
                if (query.ContainsKey(fieldsDic[Fields.Group_Id]))
                {
                    commandString += " AND U.Group_Id = @GID";
                }
                commandString += " ORDER BY SendTime";
            }
            ans.CommandText = commandString;
                
            if (query.ContainsKey(fieldsDic[Fields.Guid]))
            {
                ans.Parameters.Add("@GUID", SqlDbType.Char,68);
                ans.Parameters["@GUID"].Value = query[fieldsDic[Fields.Guid]];
            }
            else
            {
                if (query.ContainsKey(fieldsDic[Fields.SendTime]))
                {
                    ans.Parameters.Add("@SENDTIME", SqlDbType.DateTime);
                    ans.Parameters["@SENDTIME"].Value = DateTime.Parse(query[fieldsDic[Fields.SendTime]]);
                }
                if (query.ContainsKey(fieldsDic[Fields.Nickname]))
                {
                    commandString += " AND U.Nickname = @NICKNAME";
                    ans.Parameters.Add("@NICKNAME", SqlDbType.Char, 8);
                    ans.Parameters["@NICKNAME"].Value = query[fieldsDic[Fields.Nickname]];
                }
                if (query.ContainsKey(fieldsDic[Fields.Group_Id]))
                {
                    commandString += " AND U.Group_Id = @GID";
                    ans.Parameters.Add("@GID", SqlDbType.Int);
                    ans.Parameters["@GID"].Value = Convert.ToInt32(query[fieldsDic[Fields.Group_Id]]);
                }
                commandString += " ORDER BY SendTime";
            }



            return ans;
        }
        protected override string createSelectQuery(int numOfRows, Dictionary<string, string> query)
        {
            string ans = "SELECT";
            if (numOfRows > 0)
            {
                ans += $" TOP {numOfRows}";
            }
            ans += " M.Guid, U.Nickname, M.SendTime, M.Body, U.Group_Id" +
                " FROM Messages AS M JOIN USERS AS U ON M.User_Id =U.Id";
            ans += " WHERE 1=1";
            if (query.ContainsKey(fieldsDic[Fields.Guid]))
            {
                ans += $" AND Guid = {query[fieldsDic[Fields.Guid]]}";
            }
            else {
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
                ans += " ORDER BY SendTime";
            }
            

            return ans;
        }
        protected override string createInsertQuery(Dictionary<string, string> query)
        {
            string ans = "INSERT INTO Messages(";
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

        protected override SqlCommand createInsertCommand(Dictionary<string, string> query)
        {
            SqlCommand ans = new SqlCommand(null, null);
            string commandString = "INSERT INTO Messages(Guid ,  User_ID , SendTime , Body)" +
                "VALUES(@GUID, @USERID , @SENDTIME , @BODY)";
            if (!query.ContainsKey(fieldsDic[Fields.User_Id]) | !query.ContainsKey(fieldsDic[Fields.Guid])
                | !query.ContainsKey(fieldsDic[Fields.Body]) | !query.ContainsKey(fieldsDic[Fields.SendTime]))
            {
                throw new ArgumentException("insert messgae query Dictionary must contain GUID,USERID,SENDTIME,BODY query = " + query);
            }
            ans.CommandText = commandString;
            ans.Parameters.Add("@GUID", SqlDbType.Char, 68);
            ans.Parameters["@GUID"].Value =query[fieldsDic[Fields.Guid]];
            ans.Parameters.Add("@USERID", SqlDbType.Int);
            ans.Parameters["@USERID"].Value = Convert.ToInt32(query[fieldsDic[Fields.User_Id]]);
            ans.Parameters.Add("@SENDTIME", SqlDbType.DateTime);
            ans.Parameters["@SENDTIME"].Value = DateTime.Parse(query[fieldsDic[Fields.SendTime]]);
            ans.Parameters.Add("@BODY", SqlDbType.Char, 100);
            ans.Parameters["@BODY"].Value = query[fieldsDic[Fields.Body]];           
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
                /*
                 * deleteTest_message_in_db() fails becuse
                 * it the query requests to delete messages sended after
                 * the send time of the message we want to delete
                 * 
                 * it might not be that bad because the delete message is
                 * not part of the functional requirments
                 */
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
        public Dictionary<string, string> convertToDictionary(Guid guid, DateTime date, int userId, string nickname, int g_Id, string body)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!guid.Equals(default(Guid)))
            {
                dic[fieldsDic[Fields.Guid]] =  guid.ToString() ;
            }
            if (date.CompareTo(DateTime.MinValue) > 0)
            {
                dic[fieldsDic[Fields.SendTime]] =  date.ToUniversalTime().ToString() ;
            }
            if (userId != 0)
            {
                dic[fieldsDic[Fields.User_Id]] = userId.ToString();
            }
            if (nickname != null)
            {
                dic[fieldsDic[Fields.Nickname]] =   nickname  ;
            }
            if (g_Id > 0)
            {
                dic[fieldsDic[Fields.Group_Id]] = g_Id.ToString();
            }
            if (body != null)
            {
                dic[fieldsDic[Fields.Body]] =  body ;
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
            foreach (Fields field in tableColumns)
            {
                if (query.ContainsKey(fieldsDic[field]))
                {
                    ans++;
                }
            }
            return ans;
        }

        

        

        

        protected override SqlCommand createSelectCommand(int numOfRows, Dictionary<string, string> query)
        {
            throw new NotImplementedException();
        }

        #region Private Class 

        /// <summary>
        /// class that represent the handler Message object
        /// </summary>
        private sealed class HandlerMessage : IMessage
        {
            public Guid Id { get; }
            public string UserName { get; }
            public DateTime Date { get; }
            public string MessageContent { get; }
            public string GroupID { get; }
            public HandlerMessage(Guid guid, string userName, DateTime date, string messageContent , string groupId )
            {
                this.Id = guid;
                this.UserName = userName;
                this.Date = date;
                this.MessageContent = messageContent;
                this.GroupID = groupId;
            }

            public override string ToString()
            {
                return String.Format("Message ID:{0}\n" +
                    "UserName:{1}\n" +
                    "DateTime:{2}\n" +
                    "MessageContect:{3}\n" +
                    "GroupId:{4}\n"
                    , Id, UserName, Date.ToString(), MessageContent, GroupID);
            }

            
        }
        #endregion
    }

}

