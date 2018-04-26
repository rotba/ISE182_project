using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ConsoleApp1.BuissnessLayer;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class ChatRoomTests
    {
        private string dirPath;
        private ChatRoom cr;
        private ChatRoom cr2;
        private User user;
        private User user2;
        [TestInitialize]
        public void Initialize()
        {
            cr = new ChatRoom();
            cr2 = new ChatRoom();
            user = new User(15,"Rotem");
            user2 = new User(15, "Ariel");
            dirPath=
                System.IO.Directory.GetCurrentDirectory() + "\\local_files";
        }
        [TestMethod()]
        public void ChatRoomTest()
        {
            Assert.IsTrue(true);
        }
        //rotem
        [TestMethod()]
        public void loginTest()
        {
            int i;
            
            Assert.IsNull(cr.LoggedInUser,
                "User should not be initialized before logged in");
            try
            {
                cr.login(user.G_id, user.Nickname);
                Assert.Fail(
                    "Should throw exception when try to login unregistered user");
            }
            catch (ToUserException e)
            {

            }
            Assert.IsTrue(cr.LoggedInUser == null, 
                "User should not be initialized after logged in when not registered");
            cr.register(user.G_id, user.Nickname);
            cr.login(user.G_id, user.Nickname);
            Assert.IsTrue(cr.LoggedInUser.Equals(user),
                "User should be logged in successfully");
            i = 20;
            while (i--!=0) 
            {
                User newUser = new User(i,"hi "+i);
                try
                {
                    cr.login(newUser.G_id, newUser.Nickname);
                    Assert.Fail("Should throw ToUserException");
                }
                catch (ToUserException e)
                {

                }
                Assert.IsTrue(cr.LoggedInUser.Equals(user),
                    "User 15 Rotem should be logged in successfully");
            }
            cr.logout();
            try
            {
                cr.login(0, null);
                Assert.Fail("Should not allow login with null group id");
            }
            catch (ArgumentException e)
            {
                Assert.IsNull(cr.LoggedInUser, "LoggedinUser shpulf be null");
            }
            
        }
        //rotem
        [TestMethod()]
        public void logoutTest()
        {
            int i;
            Assert.IsTrue(cr.LoggedInUser == null,
                "User should not be initialized before logged in");
            cr.register(user.G_id, user.Nickname);
            cr.login(user.G_id, user.Nickname);
            Assert.IsTrue(cr.LoggedInUser.Equals(user),
                "User should be logged in successfully");
            cr.logout();
            Assert.IsTrue(cr.LoggedInUser==null,
                "User should be loggedout in successfully");
            i = 20;
            while (i-- != 0)
            {
                User newUser = new User(i, "hi " + i);
                try
                {
                    cr.login(newUser.G_id, newUser.Nickname);
                    Assert.Fail(
                        "Shouldnt allow logging a new user when one is already logged in"
                        );
                }
                catch (ToUserException e)
                {

                }
            }
        }

        [TestMethod()]
        public void exitTest()
        {
            Assert.IsTrue(true);
        }
        //rotem
        [TestMethod()]
        public void registerTest()
        {
            Assert.IsTrue(cr.LoggedInUser == null,
                "User should not be initialized before logged in");
            try
            {
                cr.login(user.G_id, user.Nickname);
                Assert.Fail(
                    "Should throw exception when try to login unregistered user"
                    );
            }
            catch (ToUserException e)
            {

            }
            Assert.IsTrue(cr.LoggedInUser == null,
                "User should be unable to login before register");
            cr.register(user.G_id,user.Nickname);
            Assert.IsTrue(cr.getUsers().Contains(user), "User should be saved in the DB");
            cr.login(user.G_id, user.Nickname);
            Assert.IsTrue(cr.LoggedInUser.Equals(user),
                "User 15 Rotem should be logged in successfully");
            int sizeOfUsersBeforeFaultRegister = cr.getUsers().Count;
            try
            {
                cr.register(0, null);
                Assert.Fail("Should not allow register with null group id");
            }
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(sizeOfUsersBeforeFaultRegister== cr.getUsers().Count, 
                    "Faulty register shouldn't change the size of the users in the DB");
            }
        }


        //rotem
        [TestMethod()]
        public void sendTest()
        {
            cr.register(user.G_id, user.Nickname);
            cr.login(user.G_id, user.Nickname);
            cr.send("Hello world");
            Message hello = null;
            foreach (Message m in cr.getMessages())
            {
                if (m.MessageContent.Equals("Hello world"))
                {
                    hello = m;
                }
            }
            
            Assert.IsNotNull(hello,
                "Message hello world should have been saved"
                );
            Assert.IsNotNull(hello.Id,
                "Message hello world GUID should not be null"
                );
            int i = 19;
            while (i!=0)
            {
                cr.send("Message number: "+i);
                i--;
            }
            i = 19;
            bool exitst = false;
            List<Message> tmp = cr.getMessages();
            while (i != 0)
            {
                foreach (Message m in cr.getMessages())
                {
                    if (m.MessageContent.Equals("Message number: " + i))
                    {
                        exitst = true;
                    }
                }
                Assert.IsTrue(exitst,"Message " + i.ToString() + " wasn't save");
                exitst = false;
                i--;
            }

        }

        //rotem
        [TestMethod()]
        public void displayNMessagesTest()
        {
            SortedSet<Message> tempMsgList = new SortedSet<Message>(new MessageDateComp());
            int i = 0;
            int magicNum = 20;
            int halfMagicNum = 10;
            cr.register(user.G_id, user.Nickname);
            cr.login(user.G_id, user.Nickname);
            cr2.register(user2.G_id, user2.Nickname);
            cr2.login(user2.G_id, user2.Nickname);
            //messages list in ChatRoom should be empty
            try
            {
                cr.displayNMessages(magicNum);
                Assert.Fail();
            }
            catch (ToUserException e) { }

            while (i < magicNum)
            {

                cr.send("" + i + "");
                cr2.send("" + i + "a");
                if ((i + 1) % (halfMagicNum / 2) == 0)
                {
                    cr.retrieveMessages(halfMagicNum);
                    cr2.retrieveMessages(halfMagicNum);
                }
                i = i + 1;
                System.Threading.Thread.Sleep(1200);
            }
            SortedSet<Message> cr2List = cr2.displayNMessages(20);
            SortedSet<Message> crList = cr.displayNMessages(20);

            for (i = 0; i < 20 ;i++ )
            {
                Message m = cr2List.ElementAt(0);
                if (crList.Contains(m))
                {
                    crList.Remove(m);
                    cr2List.Remove(m);
                }
            }
            Assert.IsTrue(crList.Count == 0, "crList should be empty after comparing");
            Assert.IsTrue(cr2List.Count == 0, "cr2List should be empty after comparing");
        }

        [TestMethod()]
        public void retrieveUserMessagesTest()
        {
            SortedSet<Message> tempMsgList = new SortedSet<Message>(new MessageDateComp());
            int i = 0;
            int magicNum = 20;
            int halfMagicNum = 10;
            cr.register(user.G_id, user.Nickname);
            cr.login(user.G_id, user.Nickname);
            cr2.register(user2.G_id, user2.Nickname);
            cr2.login(user2.G_id, user2.Nickname);
            //cr has yet sent a message
            try
            {
                cr.retrieveUserMessages(user.G_id, user.Nickname);
                Assert.Fail();
            }
            catch (ToUserException e) { }

            
            while (i < magicNum)
            {
                cr.send("" + i + "");
                if ((i + 1) % halfMagicNum/2 == 0)
                {
                    cr.retrieveMessages(halfMagicNum);
                    cr2.retrieveMessages(halfMagicNum);
                }

                i = i + 1;
                System.Threading.Thread.Sleep(2000);

            }
            SortedSet<Message> cr2List = cr2.retrieveUserMessages(user.G_id,user.Nickname);
            SortedSet<Message> crList = cr.retrieveUserMessages(user.G_id, user.Nickname);
            int crLenght = crList.Count;
            int cr2Lenght = cr2List.Count;
            Assert.IsTrue(cr2Lenght == crLenght, "both user lists should be the same lenght");
            for (i = 0; i < cr2Lenght; i++)
            {
                Message m = cr2List.ElementAt(0);
                if (crList.Contains(m))
                {
                    crList.Remove(m);
                    cr2List.Remove(m);
                }
            }
            Assert.IsTrue(crList.Count == 0, "crList should be empty after comparing");
            Assert.IsTrue(cr2List.Count == 0, "cr2List should be empty after comparing");
        }
        [TestCleanup]
        public void Cleanup()
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}