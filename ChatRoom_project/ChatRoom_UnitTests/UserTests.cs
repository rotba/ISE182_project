using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class UserTests
    {
        private User tempUser1 = new User(1, "Yossi");
        private User tempUser2 = new User(1, "Yossi");
        private User tempUser3 = new User(1, "Rotem");
        private User tempUser4 = new User(2, "Yossi");
        private User tempUser5 = new User(2, "Rotem");

  //      [TestMethod()]
  //      public void UserTest()
  //      {
  //          Assert.ThrowsException<Exception>(User tempUser = new User(1234, ""), "No username was entered, should throw an exception");
  //      }

        [TestMethod()]
        public void EqualsTest()
        {
            Assert.IsTrue(tempUser1.Equals(tempUser2), "Should be the same user");
            Assert.IsFalse(tempUser1.Equals(tempUser3), "Should be different username");
            Assert.IsFalse(tempUser1.Equals(tempUser4), "Should be different group ID");
            Assert.IsFalse(tempUser1.Equals(tempUser5), "Should be different username and group ID");
        }


        [TestMethod()]
        public void ToStringTest()
        {
            Assert.IsTrue(tempUser1.G_id == 1,"Checks if the get for group ID works");
            Assert.IsTrue(tempUser1.Nickname == "Yossi", "Checks if the get for username works");

        }

        [TestMethod()]
        public void CompareToTest()
        {
            Assert.IsTrue(tempUser1.CompareTo(tempUser2)==0, "Should be the same user");
            Assert.IsTrue(tempUser1.CompareTo(tempUser3)>0, "User3 should be first");
            Assert.IsTrue(tempUser1.CompareTo(tempUser4)<0, "User4 should be second");
            Assert.IsTrue(tempUser1.CompareTo(tempUser5)<0, "User5 should be second");
        }
    }
}