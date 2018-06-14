using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ConsoleApp1.BuissnessLayer;
using ChatRoom_project.logics;
using System.Text;
using System.Text.RegularExpressions;
using ChatRoom_project.DAL;

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
        private User user3;
        private readonly string validPW = "1234abcd";
        private readonly string saltValue = "1337";
        private Request request;


        [TestInitialize]
        public void Initialize()
        {
    
            cr = new ChatRoom();
            cr2 = new ChatRoom();
            user = new User(-1, 15, "Ariel", generateSHA256Hash(validPW)); 
            user2 = new User(-1, 15, "Rotem", generateSHA256Hash(validPW));
            user3 = new User(-1, 15, "Tomer", generateSHA256Hash(validPW));
            request = new Request();
            dirPath =
                System.IO.Directory.GetCurrentDirectory() + "\\local_files";
            Cleanup();
        }


        [TestMethod()]
        public void loginTest_before_login_LoggedInUser_is_null()
        {
            Assert.IsNull(cr.LoggedInUser,
               "User should not be initialized before logged in");
        }

        [TestMethod()]
        public void loginTest_not_registered_user_login_fails()
        {

            try
            {
                cr.login(user.G_id, user.Nickname,user.HashedPassword);
                Assert.Fail(
                    "Should throw exception when try to login unregistered user");
            }
            catch (ToUserException e)
            {

            }
               Assert.IsTrue(cr.LoggedInUser == null,
                 "User should not be initialized after logged in when not registered");
        }

        [TestMethod()]
        public void loginTest_registered_user_login_succeeds()
        {
            try
            {
                cr.register(user.G_id, user.Nickname, user.HashedPassword);
                cr.login(user.G_id, user.Nickname, user.HashedPassword);
            }
            catch (Exception e) { Assert.Fail("Should have logged in"); }
            Assert.IsTrue(cr.LoggedInUser.CompareTo(user)==0,
                   "User should be logged in successfully");
           
           
        }
        
        [TestMethod()]
        public void loginTest_Multiple_failed_logins_shouldnt_affect_current_user()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            int i;
            i = 20;
            while (i-- != 0)
            {
                User newUser = new User(-1, i, "TestUser"+i, generateSHA256Hash(validPW)); ;
                try
                {
                    cr.login(newUser.G_id, newUser.Nickname,newUser.HashedPassword);
                    Assert.Fail("Should throw ToUserException");
                }
                catch (ToUserException e)
                {

                }
                Assert.IsTrue(cr.LoggedInUser.Equals(user),
                    "User 'user' should be logged in successfully");
            }
            cr.logout();
        }

        [TestMethod()]
        public void loginTest_null_nickname_login_fails() { 
            try
            {
                cr.login(15, null, "12333");
                Assert.Fail("Should not allow login with null group id");
            }
            catch (ArgumentException e)
            {
                Assert.IsNull(cr.LoggedInUser, "LoggedinUser shpulf be null");
            }

        }

        [TestMethod()]
        public void logoutTest_successful_logout_after_logout_loggedinuser_is_null()
        {
    
            Assert.IsTrue(cr.LoggedInUser == null,
                "User should not be initialized before logged in");
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            Assert.IsTrue(cr.LoggedInUser.Equals(user),
                "User should be logged in successfully");
            cr.logout();
            Assert.IsTrue(cr.LoggedInUser == null,
                "User should be loggedout in successfully");
        }
        [TestMethod()]
        public void logoutTest_failed_logout_logout_without_login_first_throws_exception()
        {

            try
            {
                cr.logout();
                Assert.Fail(
                    "logout without initially logging in should throw user exception"
                    );
            }
            catch (ToUserException e)
            {

            }

        }

        [TestMethod()]
        public void registerTest_successfull_register()
        {
            try
            {
                cr.login(user.G_id, user.Nickname, user.HashedPassword);
                Assert.Fail("shoudlnt be allowed to login prior to register");
            }
            catch(ToUserException e)
            {

            }
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            Assert.IsTrue(cr.LoggedInUser.G_id == user.G_id & cr.LoggedInUser.Nickname.Equals(user.Nickname), "Black");
            //   Assert.IsTrue(cr.LoggedInUser.CompareTo(user)==0,
         //       "User should be logged in successfully");
    
        }

        [TestMethod()]
        public void registerTest_different_user_same_password_register()
        {
            try
            {
                cr.login(user.G_id, user.Nickname, user.HashedPassword);
                Assert.Fail("shoudlnt be allowed to login prior to register");
            }
            catch (ToUserException e)
            {

            }
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.register(user2.G_id, user2.Nickname, user2.HashedPassword);
            List<IUser> temp = request.retrieveUsers(1, user.G_id, user.Nickname);
            List<IUser> temp2 = request.retrieveUsers(1, user2.G_id, user2.Nickname);
            
            Assert.IsTrue(temp.Count==1&temp2.Count==1,
                "should be able to register different id/nickname with same password");

        }


        [TestMethod()]
        public void registerTest_register_already_registered_user_should_fail()
        {
            try
            {
                cr.login(user.G_id, user.Nickname, user.HashedPassword);
                Assert.Fail("shoudlnt be allowed to login prior to register");
            }
            catch (ToUserException e)
            {

            }
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            try
            {
                string diffPW = "00000";
                string diffHashPW = generateSHA256Hash(diffPW);
                cr.register(user.G_id, user.Nickname, diffHashPW);
                Assert.Fail("registering already registered user should throw an exception");
            }
            catch(ToUserException e_1)
            {

            }
            try
            {
                string tempPW = "abddd";
                cr.register(user.G_id, user.Nickname, generateSHA256Hash(tempPW));
                Assert.Fail("registering already registered user should throw an exception");
            }
            catch (ToUserException e_1)
            {

            }

        }

        [TestMethod()]
        public void registerTest_registeredUser_should_be_persistant()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            List<IUser> userList = request.retrieveUsers(10, user.G_id, user.Nickname);     //max count is 10
            Assert.IsTrue(userList.Count==1, "User should be saved in DB");
        }

        [TestMethod()]
        public void registerTest_null_nickname_should_fail()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            int sizeOfUsersBeforeFaultRegister = request.retrieveUsers(100000, 0, null).Count;
            try
            {
                cr.register(user.G_id, null, user.HashedPassword);
                Assert.Fail("Should not allow register with null group id");
            }
            catch (ArgumentNullException e)
            {
                Assert.IsTrue(sizeOfUsersBeforeFaultRegister == request.retrieveUsers(100000, 0, null).Count,
                    "Faulty register shouldn't change the size of the users in the DB");
            }
        }

        [TestMethod()]
        public void sendTest_sentMessage_should_be_persistant()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            cr.send("Hello world");
            Message hello = null;
            foreach (Message m in request.retrieveMessages(default(Guid),DateTime.MinValue,1,user.Nickname,user.G_id))
            {
        
                if (m.MessageContent.Contains("Hello world"))
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
        }
        [TestMethod()]
        public void sendTest_send_without_loggingin_should_throw_exception()
        {
            try
            {
                cr.send("Hello world");
                Assert.Fail("send mesaages without logging in should fail");

            }
            catch (ToUserException e)
            {

            }
        }

        [TestMethod()]
        public void sendTest_send_null_should_throw_exception()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            try
            {
                cr.send(null);
                Assert.Fail("sending null message should throw null argument exception");
            }
            catch(ArgumentNullException e)
            {
                StringAssert.Contains(e.Message, "message cannot be null", "send must throw null argument exception when reicving null argument");
            }
        }

        [TestMethod()]
        public void sendTest_message_with_over_100_chars_should_throw_exception()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            String msg = "";
            for(int i = 0; i < 151; i++)
            {
                msg = msg + 1;
            }
            try
            {
                cr.send(msg);
                Assert.Fail("100 chars in msg should throw exception");
            }
            catch(ToUserException e)
            {
                StringAssert.Contains(e.Message, "invalid message","sending message with over 100 chars should fail");
            }
        }

        [TestMethod()]
        public void sendTest_empty_msg_send_should_do_nothing()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            cr.send("");
            SortedSet<Message> tmp = request.retrieveMessages(default(Guid), DateTime.MinValue, 1, user.Nickname, user.G_id);
            Assert.IsTrue(tmp.Count == 0, "sending empty message should do nothing");

            }

        [TestMethod()]
        public void sendTest_multypul_messages_should_all_be_persistant()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            int i = 15;
            while (i != 0)
            {
                cr.send("Message number: " + i);
                i--;
            }
            i = 15;
            bool exitst = false;
            SortedSet<Message> tmp = request.retrieveMessages(default(Guid), DateTime.MinValue, i, user.Nickname, user.G_id);
            while (i != 0)
            {
                foreach (Message m in tmp)
                {
                    if (m.MessageContent.Contains("Message number: " + i))
                    {
                        exitst = true;
                    }
                }
                Assert.IsTrue(exitst, "Message " + i.ToString() + " wasn't save");
                exitst = false;
                i--;
            }

        }



        [TestMethod()]
        public void displayNMessagesTest_without_login_should_throw_exception()
        {
            try
            {
                cr.displayNMessages();
                Assert.Fail("display message shouldnt work before login");
            }
            catch(ToUserException e) {  }
        }
        [TestMethod()]
        public void displayNMessagesTest_should_throw_exception_if_there_are_no_messages()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            try
            {
                cr.displayNMessages();
                Assert.Fail("display message with no messages should throw exception");
            }
            catch (ToUserException e) { }

        }

        [TestMethod()]
        public void displayNMessagesTest_success_test()
        {
            SortedSet<Message> tempMsgList = new SortedSet<Message>(new MessageDateComp());
            int i = 0;
            int magicNum = 200;
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            cr2.register(user2.G_id, user2.Nickname, user2.HashedPassword);
            cr2.login(user2.G_id, user2.Nickname, user2.HashedPassword);

            while (i < magicNum)
            {

                cr.send("" + i + "");
                cr2.send("" + i + "a");
                System.Threading.Thread.Sleep(2000);
                i = i + 1;
            }
            SortedSet<Message> tmp = cr.displayNMessages();
            SortedSet<Message> tmp2 = cr2.displayNMessages();


            for (i = 0; i < magicNum; i++)
            {
                Message m = tmp2.ElementAt(0); 
                if (tmp.Contains(m))
                {
                    tmp.Remove(m);
                    tmp2.Remove(m);
                }
            }
            Assert.IsTrue(tmp.Count == 0, "crList should be empty after comparing");
            Assert.IsTrue(tmp2.Count == 0, "cr2List should be empty after comparing");
        }

        [TestMethod()]
        public void retrieveUserMessagesTest_witout_initailly_login_should_throw_exception()
        {

            try
            {
                SortedSet<Message> tmp = request.retrieveMessages(default(Guid), DateTime.MinValue, 100000, user.Nickname, user.G_id);
                Assert.Fail("retrieveUserMessages shouldnt work before login");
            }
            catch (ToUserException e) { }
        }


        [TestMethod()]
        public void retrieveUserMessagesTest_for_user_that_didnt_send_yet_should_be_empty()
        {
            cr.register(user.G_id, user.Nickname, user.HashedPassword);
            cr.login(user.G_id, user.Nickname, user.HashedPassword);
            try
            {
                SortedSet<Message> tmp = request.retrieveMessages(default(Guid), DateTime.MinValue, 1, user.Nickname, user.G_id);
                Assert.IsTrue(tmp.Count==0,"retrieve user mesages for user that didnt yet send message should be empty");
            }
            catch (ToUserException e) { }
        }


        [TestMethod()]
        public void password_Password_Cant_Be_Null()
        {
            string testPW = null;
            Assert.IsTrue(!verifyPW(testPW), "Password can not be null");
        }

        [TestMethod()]
        public void password_Invalid_Password_Length()
        {
            string testPW = "123";
            Assert.IsTrue(!verifyPW(testPW), "Password's length can not be lower then 4");
            testPW = "12345678901234567";
            Assert.IsTrue(!verifyPW(testPW), "Password's length can not be higher then 16");
            testPW = "";
            Assert.IsTrue(!verifyPW(testPW), "Password's length can not be 0");

        }

        [TestMethod()]
        public void password_Invalid_Password_Chars()
        {
            string testPW = "%123";
            Assert.IsTrue(!verifyPW(testPW), "% - invalid password char");
            testPW = "123455&67";
            Assert.IsTrue(!verifyPW(testPW), "& - invalid password char");
            testPW = "123456789012345א";
            Assert.IsTrue(!verifyPW(testPW), "א - invalid password char");
        }

        public void password_Valid_Password()
        {
            string testPW = "12Ab";
            Assert.IsTrue(verifyPW(testPW), "Valid Password");
            testPW = "aaaaaaaaaaaa";
            Assert.IsTrue(verifyPW(testPW), "Valid Password");
            testPW = "1A2B3c4D6794GQ0l";
            Assert.IsTrue(verifyPW(testPW), "Valid Password");
        }


        [TestCleanup]
        public void Cleanup()
        {
            cr.deleteUserAndHisMessagesForTestCleanup(user);
            cr.deleteUserAndHisMessagesForTestCleanup(user2);
            cr.deleteUserAndHisMessagesForTestCleanup(user3);
    
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
