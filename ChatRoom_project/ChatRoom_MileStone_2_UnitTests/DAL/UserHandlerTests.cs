using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1.PersistentLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PersistentLayer.Tests
{
    [TestClass()]
    public class UserHandlerTests
    { 
        /*
    
        [TestMethod()]
        public void checkIfExistsTest()
        {
            UserHandler uh = new UserHandler();
            Assert.IsTrue(uh.checkIfExists(15,"Dima"));
        }*/

        [TestMethod()]
        public void getUserHashedPWTest()
        {
            string expected = "lasttest";
            UserHandler uh = new UserHandler();
            string s = uh.getUserHashedPW(15,"Dima");
            Console.WriteLine(s);
            Assert.IsTrue(s.Length==expected.Length);
            for (int i =0; i < s.Length; i++) {
                Assert.IsTrue(s[i]==expected[i]);
            }
            Assert.IsTrue(s.Equals(expected));
        }
    }
}