using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatRoom_project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.BuissnessLayer;

namespace ChatRoom_project.DAL.Tests
{
    [TestClass()]
    public class new_MessageHandlerTests
    {
        private Message exepected_shpuldBeInDB;
        private MessageHandler handler;
        private MessageDateComp dateComp;
        private MessageUserComp userComp;
        private MessageSQLComp sqlComp;
        [TestInitialize]
        public void Initialize()
        {
            handler = new MessageHandler();
            dateComp = new MessageDateComp();
            userComp = new MessageUserComp();
            sqlComp = new MessageSQLComp();
            DateTime date = new DateTime(2012, 6, 6, 10, 34, 9);
            string content = "heyyyy brother";
            exepected_shpuldBeInDB = new Message(
                new Guid(), "Ariel", date, content, "15"
                );
            Cleanup();
        }

        [TestMethod()]
        public void insertTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void retrieveTest_retrieve_without_parameters()
        {
            
            Message result = new Message(
                handler.retrieve(1, handler.convertToDictionary(DateTime.MinValue, null, 0))[0]
                    );
            Assert.IsTrue(sqlComp.Compare(result, exepected_shpuldBeInDB)==0);
        }

        [TestMethod()]
        public void updateTest()
        {
            Assert.Fail();
        }
        [TestCleanup]
        public void Cleanup()
        {
            
        }
        
    }
}