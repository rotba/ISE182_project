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
        private int TEST_USER_ID;
        private readonly string TEST_USER_NICKNAME = "TESTUSER";
        [TestInitialize]
        public void Initialize()
        {
            TEST_USER_ID = getTestUserId();
            testUser = new User(TEST_USER_ID, 15, TEST_USER_NICKNAME);
            handler = new MessageHandler();
            dateComp = new MessageDateComp();
            userComp = new MessageUserComp();
            sqlComp = new MessageSQLComp();
            DateTime date = DateTime.Now;
            string content = "Initialize()";
            msg_in_db = new Message(
                Guid.NewGuid(),
                testUser.Nickname,
                date, content,
                testUser.G_id.ToString()
                );
            handler.insert(handler.convertToDictionary(msg_in_db, testUser.Id));
        }

        [TestMethod()]
        public void retrieveTest_without_parameters()
        {  
            Message result = new Message(
                handler.retrieve(1, handler.convertToDictionary(
                    new Guid(), DateTime.MinValue, 0 , null, 0, null))
                    [0]
                    );
            Assert.IsTrue(sqlComp.Compare(result, msg_in_db)==0);
        }
        [TestMethod()]
        public void retrieveTest_with_parameters_valid_message()
        {
            Message result = new Message(
                handler.retrieve(1,
                handler.convertToDictionary(msg_in_db,-1))
                    [0]
                    );
            Assert.IsTrue(sqlComp.Compare(result, msg_in_db) == 0);
        }
        [TestMethod()]
        public void retrieveTest_with_parameters_invalid_message()
        {

            List<IMessage> result =handler.retrieve(
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
                handler.insert(handler.convertToDictionary(test_m,testUser.Id)));
            Assert.IsTrue(sqlComp.Compare(result, test_m) == 0);
        }
        [TestMethod()]
        public void insertTest_massege_already_in_db()
        {
            try {
                Message result = new Message(
                handler.insert(handler.convertToDictionary(msg_in_db, testUser.Id)));
                Assert.Fail();
            }
            catch (SqlException sqlException) {
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
                    ).Count==0
                );
        }

        
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
        }
        private int getTestUserId()
        {
            return 6;
        }
    }
}