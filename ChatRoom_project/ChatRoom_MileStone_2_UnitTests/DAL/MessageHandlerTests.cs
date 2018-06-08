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
        private Message should_be_in_db;
        private User user_should_be_in_db;
        private MessageHandler handler;
        private MessageDateComp dateComp;
        private MessageUserComp userComp;
        private MessageSQLComp sqlComp;
        private int should_be_in_db_g_id;
        private User testUser;
        private int TEST_USER_ID;
        private readonly string TEST_USER_NICKNAME = "TESTUSER";
        [TestInitialize]
        public void Initialize()
        {
            TEST_USER_ID = getTestUserId();
            user_should_be_in_db = new User(1, 15, "Ariel");
            testUser = new User(TEST_USER_ID, 15, TEST_USER_NICKNAME);
            handler = new MessageHandler();
            dateComp = new MessageDateComp();
            userComp = new MessageUserComp();
            sqlComp = new MessageSQLComp();
            DateTime date = new DateTime(2012, 6, 6, 10, 34, 9);
            string content = "heyyyy brother";
            should_be_in_db = new Message(
                Guid.NewGuid(),
                user_should_be_in_db.Nickname,
                date, content,
                user_should_be_in_db.G_id.ToString()
                );
            int.TryParse(should_be_in_db.GroupID, out should_be_in_db_g_id);
        }

        [TestMethod()]
        public void retrieveTest_without_parameters()
        {  
            Message result = new Message(
                handler.retrieve(1, handler.convertToDictionary(
                    new Guid(), DateTime.MinValue, 0 , null, 0, null))
                    [0]
                    );
            Assert.IsTrue(sqlComp.Compare(result, should_be_in_db)==0);
        }
        [TestMethod()]
        public void retrieveTest_with_parameters_valid_message()
        {
            Message result = new Message(
                handler.retrieve(1, handler.convertToDictionary(
                    default(Guid),
                    DateTime.MinValue,
                    0,
                    should_be_in_db.UserName,
                    should_be_in_db_g_id,
                    null))
                    [0]
                    );
            Assert.IsTrue(sqlComp.Compare(result, should_be_in_db) == 0);
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
                handler.insert(handler.convertToDictionary(should_be_in_db, user_should_be_in_db.Id)));
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
                user_should_be_in_db.Nickname,
                DateTime.Now,
                "insertTest_massege_already_in_db()",
                id_of_user_not_in_db
                );
            try
            {
                Message result = new Message(
                handler.insert(handler.convertToDictionary(should_be_in_db, user_should_be_in_db.Id)));
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
                Guid.NewGuid(), testUser.Nickname, DateTime.MinValue, "deleteTest_message_in_db", testUser.G_id.ToString()
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