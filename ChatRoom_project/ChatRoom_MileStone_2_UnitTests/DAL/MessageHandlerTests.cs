using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatRoom_project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.BuissnessLayer;
using MileStoneClient.CommunicationLayer;
using ChatRoom_project.logics;
using System.Data.SqlClient;

namespace ChatRoom_project.DAL.Tests
{
    [TestClass()]
    public class MessageHandlerTests
    {
        private Message msg_in_db;
        private MessageHandler handler;
        private MessageDateComp dateComp;
        private MessageUserComp userComp;
        private MessageSQLComp sqlComp;
        private User testUser;
        private User testUserA;
        private User testWatchUser;
        private int TEST_USER_ID;
        private readonly string TEST_USER_NICKNAME = "TESTUSER";
        private readonly string TEST_USERA_NICKNAME = "TESTUSEA";
        private readonly string TEST_WATCH_USER_NICKNAME = "WATCH";
        private DateTime localDate;
        private bool iWantToWatch;
        [TestInitialize]
        public void Initialize()
        {
            iWantToWatch = false;
            TEST_USER_ID = getTestUserId();
            testUser = new User(TEST_USER_ID, 15, TEST_USER_NICKNAME, "496351");
            testWatchUser = new User(10, 15, TEST_WATCH_USER_NICKNAME, "496351");
            testUserA = new User(13, 15, TEST_USERA_NICKNAME, "496351");
            handler = new MessageHandler();
            dateComp = new MessageDateComp();
            userComp = new MessageUserComp();
            sqlComp = new MessageSQLComp();
            localDate = DateTime.Now.ToLocalTime();
            string content = "Initialize()";
            msg_in_db = new Message(
                Guid.NewGuid(),
                testUser.Nickname,
                localDate,
                content,
                testUser.G_id.ToString()
                );
            handler.insert(handler.convertToDictionary(msg_in_db, testUser.Id));
        }

        [TestMethod()]
        public void retrieveTest_without_parameters()
        {
            Message result = new Message(
                handler.retrieve(1, handler.convertToDictionary(
                    msg_in_db.Id, DateTime.MinValue, 0, null, 0, null))
                    [0]
                    );
            Assert.IsTrue(sqlComp.Compare(result, msg_in_db) == 0);
        }
        [TestMethod()]
        public void retrieveTest_check_timefilter()
        {
            //User currTestUser = testUser;
            User currTestUser = testUserA;
            //User currTestUser = testWatchUser;
            /*
             *for clarity, messages time stamped between 1 to 10 (not 0 to 9)
             * bottom line - only 6,7,8,9,10 should be in DB
             */
            int retriveMessagesNewerThanMe = 4;
            Message[] messages = new Message[10];
            DateTime[] dates = new DateTime[10];
            for (int i =1; i<=messages.Length; i++) {
                dates[i-1] =new DateTime(2018,2,12,13,0,i);
                messages[i-1] = new Message(
                    Guid.NewGuid(),
                    currTestUser.Nickname,
                    dates[i-1],
                    $"MESSAGE NUMBER {i} retrieveTest_check_timefilter()",
                    currTestUser.G_id.ToString()
                    );
                handler.insert(handler.convertToDictionary(messages[i-1], currTestUser.Id));
            }
            List<IMessage> result =handler.retrieve(-1, 
                handler.convertToDictionary(
                    default(Guid),
                    dates[retriveMessagesNewerThanMe],
                    0,
                    TEST_USERA_NICKNAME,
                    0,
                    null)
                    );
            List<Message> messageResult = new List<Message>();
            foreach (IMessage imessage in result) {
                messageResult.Add(new Message(imessage));
            }
            for (int i = 0; i < retriveMessagesNewerThanMe; i++)
            {
                Assert.IsFalse(messageResult.Contains(messages[i]));
            }
            for (int i = retriveMessagesNewerThanMe+1; i < messages.Length; i++)
            {
                Assert.IsTrue(messageResult.Contains(messages[i]));
            }
        }
        [TestMethod()]
        public void retrieveTest_with_parameters_valid_message()
        {
            Message result = new Message(
                handler.retrieve(1,
                handler.convertToDictionary(msg_in_db, -1))
                    [0]
                    );
            Assert.IsTrue(sqlComp.Compare(result, msg_in_db) == 0);
        }
        [TestMethod()]
        public void retrieveTest_message_should_be_retrieved_with_local_time_zone()
        {
            Message result = new Message(
                handler.retrieve(1,
                handler.convertToDictionary(msg_in_db, -1))
                    [0]
                    );
            Assert.IsTrue(result.Date.ToString().Equals(localDate.ToString()));
        }
        [TestMethod()]
        public void retrieveTest_with_parameters_invalid_message()
        {

            List<IMessage> result = handler.retrieve(
                1, handler.convertToDictionary(
                    new Guid(), DateTime.MinValue, 0, "NotInDB", 0, null)
                    );
            Assert.IsTrue(result.Count == 0);
        }
        [TestMethod()]
        public void insertTest_valid_message()
        {
            Message test_m = new Message(
                Guid.NewGuid(), testUser.Nickname, DateTime.Now, "insertTest_valid_message()", testUser.G_id.ToString()
                );
            Message result = new Message(
                handler.insert(handler.convertToDictionary(test_m, testUser.Id)));
            Assert.IsTrue(sqlComp.Compare(result, test_m) == 0);
        }
        [TestMethod()]
        public void insertTest_massege_already_in_db()
        {
            try
            {
                Message result = new Message(
                handler.insert(handler.convertToDictionary(msg_in_db, testUser.Id)));
                Assert.Fail();
            }
            catch (SqlException sqlException)
            {
            }
        }
        [TestMethod()]
        public void insertTest_user_not_in_db()
        {
            string id_of_user_not_in_db = "1000";
            Message user_not_be_in_db = new Message(
                Guid.NewGuid(),
                testUser.Nickname,
                DateTime.Now,
                "insertTest_massege_already_in_db()",
                id_of_user_not_in_db
                );
            try
            {
                Message result = new Message(
                handler.insert(handler.convertToDictionary(msg_in_db, testUser.Id)));
                Assert.Fail();
            }
            catch (SqlException sqlException)
            {
            }
        }
        [TestMethod()]
        public void deleteTest_message_in_db()
        {
            Message test_m = new Message(
                Guid.NewGuid(), testUser.Nickname, DateTime.Now, "deleteTest_message_in_db", testUser.G_id.ToString()
                );
            handler.insert(handler.convertToDictionary(test_m, TEST_USER_ID));
            handler.delete(handler.convertToDictionary(test_m, TEST_USER_ID));
            Assert.IsTrue(
                handler.retrieve(
                    -1, handler.convertToDictionary(test_m, TEST_USER_ID)
                    ).Count == 0
                );
        }
        /*
         * This test correctness should be examined in the db
         */
        /*
       [TestMethod()]
       public void insertDBTest_valid_message()
       {
           DateTime localNow = DateTime.Now.ToLocalTime();
           Message test_m = new Message(
               Guid.NewGuid(), testWatchUser.Nickname, localNow,
               "insertDBTest_valid_message" +
               $"The expected time is: {localNow.ToUniversalTime()}",
               testWatchUser.G_id.ToString()
               );
           Message result = new Message(
               handler.insert(handler.convertToDictionary(test_m, testWatchUser.Id)));
       }
       */
        [TestCleanup]
        public void Cleanup()
        {
            Message tester_msg = new Message(
                new Guid(),
                TEST_USER_NICKNAME,
                DateTime.MinValue,
                null,
                null
                );
            handler.delete(
                handler.convertToDictionary(tester_msg, -1)
                );
            Message testera_msg = new Message(
                new Guid(),
                TEST_USERA_NICKNAME,
                DateTime.MinValue,
                null,
                null
                );
            handler.delete(
                handler.convertToDictionary(testera_msg, -1)
                );

            if (!iWantToWatch) {
                Message watch_msg = new Message(
                new Guid(),
                TEST_WATCH_USER_NICKNAME,
                DateTime.MinValue,
                null,
                null
                );
                handler.delete(
                    handler.convertToDictionary(watch_msg, -1)
                );
            }
            
        }
        private int getTestUserId()
        {
            return 6;
        }
    }
}