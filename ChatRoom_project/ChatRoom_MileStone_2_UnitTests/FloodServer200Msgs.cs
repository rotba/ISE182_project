using System;
using System.Text;
using System.Text.RegularExpressions;
using ChatRoom_project.logics;
using ConsoleApp1.BuissnessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatRoom_MileStone_2_UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        private string dirPath;
        private ChatRoom cr;
        private ChatRoom cr2;
        private User user;
        private User user2;
        private User user3;
        private readonly string validPW = "1234abcd";
        private readonly string saltValue = "1337";
        private Request request;


        [TestInitialize]
        public void Initialize()
        {

            cr = new ChatRoom();
            cr2 = new ChatRoom();
            user = new User(-1, 15, "TESTTEST", generateSHA256Hash(validPW));
            user2 = new User(-1, 15, "Rotem", generateSHA256Hash(validPW));
            user3 = new User(-1, 15, "Tomer", generateSHA256Hash(validPW));
            request = new Request();
           
        }
        [TestMethod]
        public void flood200Msgs()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            for (int i = 0; i < 250; i++)
                cr.send("Test num " + i);
        }

        private string generateSHA256Hash(string input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + saltValue);
            System.Security.Cryptography.SHA256Managed sha256HashString =
                new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256HashString.ComputeHash(bytes);
            return byteArrayToHexString(hash);
        }

        private string byteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        private bool verifyPW(string pw)
        {
            if (pw == null)
            {
                return false;
            }
            if (pw == "" | pw.Length < 4 | pw.Length > 16)
            {
                return false;
            }

            if (!Regex.IsMatch(pw, @"^[a-zA-Z0-9]+$"))
            {
                return false;
            }
            return true;

        }
    }
}
