using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleApp1.UnitTest
{
    [TestClass]
    public class ChatRoomTest
    {
        [TestMethod]
        //tests register function
        public void TestRegister()
        {
            ChatRoom chtrm = new ChatRoom();
            Assert.IsTrue(chtrm.register(555, "test1"));
            Assert.IsFalse(chtrm.register(555, "test1"));
            Assert.IsTrue(chtrm.register(556, "test1"));
            Assert.IsTrue(chtrm.register(556, "test2"));
           

        }
        [TestMethod]
        public void TestLogin()
        {
            ChatRoom chtrm = new ChatRoom();
            Assert.IsTrue(chtrm.login(555, "test1"));
            Assert.IsFalse(chtrm.login(555, "test5"));
            Assert.IsFalse(chtrm.login(557, "test1"));
            User u = new User(555, "test1");
            Assert.IsTrue(chtrm.LoggedInUser.Equals(u));
            Assert.IsTrue(chtrm.login(556, "test2"));
            u = new User(556, "test2");
            Assert.IsTrue(chtrm.LoggedInUser.Equals(u));
        }
    }
}
