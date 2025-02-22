﻿using ChatRoom_project.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom_project.logics
{
    public class User: IComparable, IUser
    {
        private string nickname;
        private string hashedPassword;
        public string HashedPassword { get => hashedPassword; private set => hashedPassword = value; }
        private int id;
        public int Id { get => id; private set { id = value; } }
        private int g_id;
        public int G_id { get => g_id; private set => g_id = value; }

        public string Nickname { get => nickname; } 

        public User(int Id, int g_id , string nickname, string hashedPW)
        {
            if (nickname == null)
                throw new ArgumentNullException("nickname cannot be null");
            if (nickname == "")
                throw new ArgumentException("nickname cannot be empty");
            G_id = g_id;
            this.nickname = nickname;
            this.Id = Id;
            this.HashedPassword = hashedPW;
        }
        /*
        //copy constructor
        public User(User user)
        {
            if (user==null)
            {
                return;
            }
            G_id = user.g_id;
            this.nickname = user.nickname;
            Id = user.Id;
            HashedPassword = user.hashedPassword;
            
        }
        */
        //copy constructor
        public User(IUser user)
        {
            if (user == null)
            {
                return;
            }
            G_id = user.G_id;
            this.nickname = user.Nickname;
            Id = user.Id;
            HashedPassword = user.HashedPassword;

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
