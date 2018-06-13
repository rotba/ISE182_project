//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.IO;
//using ConsoleApp1.BuissnessLayer;
//using ChatRoom_project.logics;

//namespace ConsoleApp1.Tests
//{
//    [TestClass()]
//    public class ChatRoomTests
//    {
//        private string dirPath;
//        private ChatRoom cr;
//        private ChatRoom cr2;
//        private User user;
//        private User user2;
//        [TestInitialize]
//        public void Initialize()
//        {
            
//            cr = new ChatRoom();
//            cr2 = new ChatRoom();
//            user = new User(1,15,"TESTUSER");
//            //user = cr.register(15, "TESTUSER");
//            user2 = new User(2,15, "Ariel");
//            dirPath=
//                System.IO.Directory.GetCurrentDirectory() + "\\local_files";
//            Cleanup();
//        }

        
//        [TestMethod()]
//        public void loginTest_before_login_LoggedInUser_is_null()
//        {
//            Assert.IsNull(cr.LoggedInUser,
//               "User should not be initialized before logged in");
//        }

//        [TestMethod()]
//        public void loginTest_not_registered_user_login_fails()
//        {
           
//            try
//            {
//                cr.login(user.G_id, user.Nickname);
//                Assert.Fail(
//                    "Should throw exception when try to login unregistered user");
//            }
//            catch (ToUserException e)
//            {

//            }

//            Assert.IsTrue(cr.LoggedInUser == null,
//                "User should not be initialized after logged in when not registered");
//        }

//        [TestMethod()]
//        public void loginTest_registered_user_login_succeeds()
//        {

//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            Assert.IsTrue(cr.LoggedInUser.Equals(user),
//                "User should be logged in successfully");
//        }
//        [TestMethod()]
//        public void loginTest_Multiple_failed_logins_shouldnt_affect_current_user()
//        {

//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            int i;
//            i = 20;
//            while (i-- != 0)
//            {
//                User newUser = new User(1,i, "hi " + i);
//                try
//                {
//                    cr.login(newUser.G_id, newUser.Nickname);
//                    Assert.Fail("Should throw ToUserException");
//                }
//                catch (ToUserException e)
//                {

//                }
//                Assert.IsTrue(cr.LoggedInUser.Equals(user),
//                    "User 15 Rotem should be logged in successfully");
//            }
//            cr.logout();
//        }
//        [TestMethod()]
//        public void loginTest_null_nickname_login_fails() { 
//            try
//            {
//                cr.login(0, null);
//                Assert.Fail("Should not allow login with null group id");
//            }
//            catch (ArgumentException e)
//            {
//                Assert.IsNull(cr.LoggedInUser, "LoggedinUser shpulf be null");
//            }

//        }

//        [TestMethod()]
//        public void logoutTest_successful_logout_after_logout_loggedinuser_is_null()
//        {
            
//            Assert.IsTrue(cr.LoggedInUser == null,
//                "User should not be initialized before logged in");
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            Assert.IsTrue(cr.LoggedInUser.Equals(user),
//                "User should be logged in successfully");
//            cr.logout();
//            Assert.IsTrue(cr.LoggedInUser == null,
//                "User should be loggedout in successfully");
//        }
//        [TestMethod()]
//        public void logoutTest_failed_logout_logout_without_login_first_throws_exception()
//        {

//            try
//            {
//                cr.logout();
//                Assert.Fail(
//                    "logout without initially logging in should throw user exception"
//                    );
//            }
//            catch (ToUserException e)
//            {

//            }

//        }

//        [TestMethod()]
//        public void registerTest_successfull_register()
//        {
//            try
//            {
//                cr.login(user.G_id, user.Nickname);
//                Assert.Fail("shoudlnt be allowed to login prior to register");
//            }
//            catch(ToUserException e)
//            {

//            }
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            Assert.IsTrue(cr.LoggedInUser.Equals(user),
//                "User should be logged in successfully");
            
//        }

//        [TestMethod()]
//        public void registerTest_register_already_registered_user_should_fail()
//        {
//            try
//            {
//                cr.login(user.G_id, user.Nickname);
//                Assert.Fail("shoudlnt be allowed to login prior to register");
//            }
//            catch (ToUserException e)
//            {

//            }
//            cr.register(user.G_id, user.Nickname);
//            try
//            {
//                cr.register(user.G_id, user.Nickname);
//                Assert.Fail("registering already registered user should throw an exception");
//            }
//            catch(ToUserException e_1)
//            {
                
//            }

//        }

//        [TestMethod()]
//        public void registerTest_registeredUser_should_be_persistant()
//        {
//            cr.register(user.G_id, user.Nickname);
//            Assert.IsTrue(cr.getUsers().Contains(user), "User should be saved in DB");
//        }

//        [TestMethod()]
//        public void registerTest_null_nickname_should_fail()
//        {
//            cr.register(user.G_id, user.Nickname);
//            int sizeOfUsersBeforeFaultRegister = cr.getUsers().Count;
//            try
//            {
//                cr.register(0, null);
//                Assert.Fail("Should not allow register with null group id");
//            }
//            catch (ArgumentNullException e)
//            {
//                Assert.IsTrue(sizeOfUsersBeforeFaultRegister == cr.getUsers().Count,
//                    "Faulty register shouldn't change the size of the users in the DB");
//            }
//        }

//        [TestMethod()]
//        public void sendTest_sentMessage_should_be_persistant()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            cr.send("Hello world");
//            Message hello = null;
//            foreach (Message m in cr.getMessages())
//            {
                
//                if (m.MessageContent.Contains("Hello world"))
//                {
//                    hello = m;
//                }
//            }

//            Assert.IsNotNull(hello,
//                "Message hello world should have been saved"
//                );
//            Assert.IsNotNull(hello.Id,
//                "Message hello world GUID should not be null"
//                );
//        }
//        [TestMethod()]
//        public void sendTest_send_without_loggingin_should_throw_exception()
//        {
//            try
//            {
//                cr.send("Hello world");
//                Assert.Fail("send mesaages without logging in should fail");

//            }
//            catch (ToUserException e)
//            {

//            }
//        }

//        [TestMethod()]
//        public void sendTest_send_null_should_throw_exception()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            try
//            {
//                cr.send(null);
//                Assert.Fail("sending null message should throw null argument exception");
//            }
//            catch(ArgumentNullException e)
//            {
//                StringAssert.Contains(e.Message, "message cannot be null", "send must throw null argument exception when reicving null argument");
//            }
//        }

//        [TestMethod()]
//        public void sendTest_message_with_over_100_chars_should_throw_exception()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            String msg = "";
//            for(int i = 0; i < 151; i++)
//            {
//                msg = msg + 1;
//            }
//            try
//            {
//                cr.send(msg);
//                Assert.Fail("100 chars in msg should throw exception");
//            }
//            catch(ToUserException e)
//            {
//                StringAssert.Contains(e.Message, "invalid message","sending message with over 100 chars should fail");
//            }
//        }

//        [TestMethod()]
//        public void sendTest_empty_msg_send_should_do_nothing()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            cr.send("");
//            List<Message> tmp = cr.getMessages();
//            Assert.IsTrue(tmp.Count == 0, "sending empty message should do nothing");

//            }

//        [TestMethod()]
//        public void sendTest_multypul_messages_should_all_be_persistant()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            int i = 15;
//            while (i != 0)
//            {
//                cr.send("Message number: " + i);
//                i--;
//            }
//            i = 15;
//            bool exitst = false;
//            List<Message> tmp = cr.getMessages();
            
//            while (i != 0)
//            {
//                foreach (Message m in tmp)
//                {
//                    if (m.MessageContent.Contains("Message number: " + i))
//                    {
//                        exitst = true;
//                    }
//                }
//                Assert.IsTrue(exitst, "Message " + i.ToString() + " wasn't save");
//                exitst = false;
//                i--;
//            }

//        }

//        [TestMethod()]
//        public void displayNMessagesTest_with_invalid_input_should_throw_exception()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            try
//            {
//                cr.displayNMessages(-1);
//                Assert.Fail("display message must recieve non negative integer");
//            }
//            catch (ArgumentOutOfRangeException e)
//            {
//                StringAssert.Contains(e.Message, "display message must recieve non negative integer", "display messages with negative integer should throw exception");
//            }
//        }

//        [TestMethod()]
//        public void displayNMessagesTest_without_login_should_throw_exception()
//        {
//            try
//            {
//                cr.displayNMessages(20);
//                Assert.Fail("display message shouldnt work before login");
//            }
//            catch(ToUserException e) {  }
//        }
//        [TestMethod()]
//        public void displayNMessagesTest_should_throw_exception_if_there_are_no_messages()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            try
//            {
//                cr.displayNMessages(20);
//                Assert.Fail("display message with no messages should throw exception");
//            }
//            catch (ToUserException e) { }

//        }

//        [TestMethod()]
//        public void displayNMessagesTest_success_test()
//        {
//            SortedSet<Message> tempMsgList = new SortedSet<Message>(new MessageDateComp());
//            int i = 0;
//            int magicNum = 20;
//            int halfMagicNum = 10;
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            cr2.register(user2.G_id, user2.Nickname);
//            cr2.login(user2.G_id, user2.Nickname);
            
//            while (i < magicNum)
//            {

//                cr.send("" + i + "");
//                cr2.send("" + i + "a");
//                if ((i + 1) % (halfMagicNum / 2) == 0)
//                {
//                    cr.retrieveMessages(halfMagicNum);
//                    cr2.retrieveMessages(halfMagicNum);
//                }
//                i = i + 1;
//                System.Threading.Thread.Sleep(1200);
//            }
//            SortedSet<Message> cr2List = cr2.displayNMessages(20);
//            SortedSet<Message> crList = cr.displayNMessages(20);

//            for (i = 0; i < 20; i++)
//            {
//                Message m = cr2List.ElementAt(0);
//                if (crList.Contains(m))
//                {
//                    crList.Remove(m);
//                    cr2List.Remove(m);
//                }
//            }
//            Assert.IsTrue(crList.Count == 0, "crList should be empty after comparing");
//            Assert.IsTrue(cr2List.Count == 0, "cr2List should be empty after comparing");
//        }

//        [TestMethod()]
//        public void retrieveUserMessagesTest_witout_initailly_login_should_throw_exception()
//        {

//            try
//            {
//                cr.retrieveUserMessages(user.G_id,user.Nickname);
//                Assert.Fail("retrieveUserMessages shouldnt work before login");
//            }
//            catch (ToUserException e) { }
//        }

//        [TestMethod()]
//        public void retrieveUserMessagesTest_for_null_nickname_should_throw_exception()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            try
//            {
//                cr.retrieveUserMessages(user.G_id, null);
//                Assert.Fail("retirive use messages should throw exception for null argument");
//            }
//            catch (ArgumentNullException e)
//            {
//                StringAssert.Contains(e.Message, "nickname cannot be null", "null nickname should throw exception");
//            }
//        }

//        [TestMethod()]
//        public void retrieveUserMessagesTest_for_user_that_didnt_send_yet_should_throw_exception()
//        {
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            try
//            {
//                cr.retrieveUserMessages(user.G_id, user.Nickname);
//                Assert.Fail("retrieve user mesages for user that didnt yet send message should throw exception");
//            }
//            catch (ToUserException e) { }
//        }

//        [TestMethod()]
//        public void retrieveUserMessagesTest_success_test()
//        {

//            SortedSet<Message> tempMsgList = new SortedSet<Message>(new MessageDateComp());
//            int i = 0;
//            int magicNum = 20;
//            int halfMagicNum = 10;
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            cr2.register(user2.G_id, user2.Nickname);
//            cr2.login(user2.G_id, user2.Nickname);
           
//            while (i < magicNum)
//            {
//                cr.send("" + i + "");
//                if ((i + 1) % halfMagicNum / 2 == 0)
//                {
//                    cr.retrieveMessages(halfMagicNum);
//                    cr2.retrieveMessages(halfMagicNum);
//                }

//                i = i + 1;
//                System.Threading.Thread.Sleep(2000);

//            }
//            SortedSet<Message> cr2List = cr2.retrieveUserMessages(user.G_id, user.Nickname);
//            SortedSet<Message> crList = cr.retrieveUserMessages(user.G_id, user.Nickname);
//            int crLenght = crList.Count;
//            int cr2Lenght = cr2List.Count;
//            Assert.IsTrue(cr2Lenght == crLenght, "both user lists should be the same lenght");
//            for (i = 0; i < cr2Lenght; i++)
//            {
//                Message m = cr2List.ElementAt(0);
//                if (crList.Contains(m))
//                {
//                    crList.Remove(m);
//                    cr2List.Remove(m);
//                }
//            }
//            Assert.IsTrue(crList.Count == 0, "crList should be empty after comparing");
//            Assert.IsTrue(cr2List.Count == 0, "cr2List should be empty after comparing");
//        }

//        [TestMethod()]
//        public void new_retrieveMessagesTest()
//        {
//            SortedSet<Message> msgs;
//            cr.register(user.G_id, user.Nickname);
//            cr.login(user.G_id, user.Nickname);
//            cr.retrieveMessages(10);
//            //cr.retrieveMessages(10, null, 0);
//            msgs = cr.displayNMessages(10);
//            Assert.IsTrue(msgs.Min.MessageContent.Equals("heyyyy brother"));
//        }


//        [TestCleanup]
//        public void Cleanup()
//        {
//            cr.deleteUserAndHisMessagesForTestCleanup(user);
//            cr.deleteUserAndHisMessagesForTestCleanup(user2);

//            DirectoryInfo di = new DirectoryInfo(dirPath);
            
//            foreach (FileInfo file in di.GetFiles())
//            {
//                file.Delete();
//            }
//            foreach (DirectoryInfo dir in di.GetDirectories())
//            {
//                dir.Delete(true);
//            }
            
//        }
//    }
//}