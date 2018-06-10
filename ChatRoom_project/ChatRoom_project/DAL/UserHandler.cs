using ChatRoom_project.logics;
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
     /*   enum Fields { Id, Group_Id, Nickname, Password};
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

        protected override string createQuery(int numOfRows, Dictionary<string, string> query)
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
            if (number > 0)
            {
                ans += $" LIMIT {number}";
            }
            return ans;
        }*/
    }
}
