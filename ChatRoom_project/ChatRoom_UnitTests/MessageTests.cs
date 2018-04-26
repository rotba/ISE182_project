using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.BuissnessLayer;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class MessageTests
    {
        private Message tempMsg = new Message(new Guid(), "Yossi", new DateTime(), "Hello", "1234");
        private Message noUserName = new Message(new Guid(), "", new DateTime(), "Hello", "1234");
        private Message noContent = new Message(new Guid(), "Yossi", new DateTime(), "", "1234");
        private Message noID = new Message(new Guid(), "Yossi", new DateTime(), "Hello", "");
        private Message noDigitID = new Message(new Guid(), "Yossi", new DateTime(), "Hello", "12a3");
//        [TestMethod()]
        //checks if a message was created with an invailed username, ID or message content 
//        public void MessageTest()
//        {
//            Assert.IsNotInstanceOfType(noUserName, typeof(Message), "Empty username, shouldn't be able to create a message");
//            Assert.IsNotInstanceOfType(noContent, typeof(Message), "No content to message, shouldn't be able to create a message");
//            Assert.IsNotInstanceOfType(noID, typeof(Message), "Empty Id, shouldn't be able to create a message");
//            Assert.IsNotInstanceOfType(noDigitID, typeof(Message), "ID doesn't include only digits, shouldn't be able to create a message");
//        }


        [TestMethod()]

        public void ToStringTest()
        {
            Assert.IsTrue(tempMsg.GroupID == "1234");
            Assert.IsTrue(tempMsg.UserName == "Yossi");
            Assert.IsTrue(tempMsg.MessageContent == "Hello");
            Assert.IsFalse(tempMsg.ToString()=="asd","Should be false because the it is not the same");     
         }

        [TestMethod()]
        public void EqualsTest()
        {
            Message tempMsg2 = new Message(new Guid(), "Yossi", new DateTime(), "Hello", "1234");
            Assert.IsTrue(tempMsg.Equals(tempMsg), "Should be the same message");
            Assert.IsFalse(tempMsg.Equals(tempMsg2), "Should be different Guid");
            Assert.IsFalse(tempMsg.Equals('b'), "Must compare with another Message type");
        }
    }
}