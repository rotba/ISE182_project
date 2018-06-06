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
    public class MessageHandler: Handler<IMessage>
    {
        enum Fields{ SendTime, Nickname, Group_Id };
        private static readonly Dictionary<Fields, string> fieldsDic = new Dictionary<Fields, string>()
        {
            {Fields.SendTime, "SendTime"},
            {Fields.Nickname, "Nickname"},
            {Fields.Group_Id, "Group_Id"}
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

        protected override string createQuery(int numOfRows, Dictionary<string, string> query)
        {
            string ans = "SELECT";
            if (numOfRows>0) {
                ans += $" TOP {numOfRows}";
            }
            ans+=" U.Nickname, M.SendTime, M.Body, U.Group_Id" +
                " FROM Messages AS M JOIN USERS AS U ON M.User_Id =U.Id";
            ans += " WHERE 1=1";
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
            
            return ans;
        }
        public Dictionary<string, string> convertToDictionary(DateTime date, string nickname, int g_Id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (date.CompareTo(DateTime.MinValue) < 0)
            {
                
                dic[fieldsDic[Fields.SendTime]] = date.ToString();
            }
            if (nickname != null)
            {
                dic[fieldsDic[Fields.Nickname]] = nickname;
            }
            if (g_Id > 0)
            {
                dic[fieldsDic[Fields.Group_Id]] = g_Id.ToString();
            }
            return dic;
        }
    }
}
