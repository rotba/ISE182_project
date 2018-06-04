using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.BuissnessLayer
{
    [Serializable]
    public class User : IComparable
    {

        private int g_id;
        
        private string nickname;

        private string pw;

        public int G_id { get => g_id; private set => g_id = value; }
        public string Nickname { get => nickname; private set => nickname = value; }
        public string Pw { get => pw; private set => pw = value; }

        public User(int g_id,string nickname,string pw)
        {
            if (nickname == null)
                throw new ArgumentNullException("nickname cannot be null");
            if (nickname == "")
                throw new ArgumentException("nickname cannot be empty");
            if (pw == null)
                throw new ArgumentNullException("password cannot be null");
            if (pw == "")
                throw new ArgumentException("password cannot be empty");
            G_id = g_id;
            Nickname = nickname;
            Pw = pw;
        }
        //copy constructor
        public User(User user)
        {
            if (user==null)
            {
                return;
            }
            G_id = user.g_id;
            Nickname = user.nickname;
        }
        // 2 users are equal if both g_id and nickname are equal.
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            User u = (User)obj;
            return (g_id == u.g_id) && (nickname == u.nickname);
        }
        public override int GetHashCode()
        {
            return g_id.GetHashCode() ^ nickname.GetHashCode();
        }
        public override string ToString()
        {
            return "User: G_id = " + G_id + " Nickname: " + Nickname;
        }
        // compares by g_id, if equal by nickname lexicographic order.
        public int CompareTo(object obj)
        {
            /*
             * returns -1 if this < obj
             * 0 if this = obj
             * 1 if this > obj
             */
            if (obj == null) return 1;

            User otherUser = obj as User;
            if (otherUser != null)
            {
                if (this.g_id.CompareTo(otherUser.g_id) != 0)
                    return this.g_id.CompareTo(otherUser.g_id);
                else
                    return this.nickname.CompareTo(otherUser.nickname);
            }
            else
                throw new ArgumentException("Object is not a User");
        }
    }
}
