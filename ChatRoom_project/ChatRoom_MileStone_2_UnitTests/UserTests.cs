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
        public void EqualsTest_with_null_returns_false()
        {
            Assert.IsFalse(tempUser1.Equals(null), "equals with null should return false");
        }

        [TestMethod()]
        public void EqualsTest_with_not_User_object_returns_false()
        {
            Assert.IsFalse(tempUser1.Equals(new ChatRoom()), "equals with object other than user should return false");
        }

        [TestMethod()]
        public void EqualsTest_equal_user_test_succsess()
        {
            Assert.IsTrue(tempUser1.Equals(tempUser2), "Should be the same user");
        }

        [TestMethod()]
        public void EqualsTest_different_username_returns_false()
        {
            Assert.IsFalse(tempUser1.Equals(tempUser3), "Should be different username");
        }

        [TestMethod()]
        public void EqualsTest_different_groupID_returns_false()
        {
            Assert.IsFalse(tempUser1.Equals(tempUser4), "Should be different group ID");
            Assert.IsFalse(tempUser1.Equals(tempUser5), "Should be different username and group ID");
        }
        [TestMethod()]
        public void EqualsTest_different_IDandNick_returns_false()
        {
            Assert.IsFalse(tempUser1.Equals(tempUser5), "Should be different username and group ID");
        }

        [TestMethod()]
        public void CompareToTest_same_user_returns_0()
        {
            Assert.IsTrue(tempUser1.CompareTo(tempUser2) == 0, "Should be the same user");
        }


        [TestMethod()]
        public void CompareToTest_same_groupID_diff_nick_sorts_by_nick()
        {
            Assert.IsTrue(tempUser1.CompareTo(tempUser3) > 0, "User3 should be first");
        }

        [TestMethod()]
        public void CompareToTest_same_nick_diff_groupID_sorts_by_group()
        {
            Assert.IsTrue(tempUser1.CompareTo(tempUser4) < 0, "User4 should be second");
        }

        [TestMethod()]
        public void CompareToTest_altogether_diff_users()
        {
            Assert.IsTrue(tempUser1.CompareTo(tempUser5) < 0, "User5 should be second");
        }

        [TestMethod()]
        public void CompareToTest_null_compare()
        {
            Assert.IsTrue(tempUser1.CompareTo(null) > 0, "null comes after any user");
        }

        [TestMethod()]
        public void CompareToTest_notUser_compare_throws_exception()
        {
            try
            {
                tempUser1.CompareTo(5);
                Assert.Fail("user can compare to user or null object only");
            }
            catch (ArgumentException e)
            {

            }
        }


    }
}