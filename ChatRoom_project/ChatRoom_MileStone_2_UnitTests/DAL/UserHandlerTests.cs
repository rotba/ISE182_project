using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1.PersistentLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.BuissnessLayer;

namespace ConsoleApp1.PersistentLayer.Tests
{
    [TestClass()]
    public class UserHandlerTests
    {


        [TestMethod()]
        public void checkIfExistsTest()
        {
            UserHandler uh = new UserHandler();
            Assert.IsTrue(uh.checkIfExists(15, "Dima"));
        }

        //needs to change genrateHash256 and createSalt to public in ChatRoom
        
       [TestMethod()]
       public void getUserHashedPWTest()
       {
           string originalPw = "yami";
           ChatRoom cr = new ChatRoom();
           string result = cr.getHashPWForTests(originalPw);
          // string expected = "ec837e92c6c9593684a14495a6297916b38249668dcb4d1e254259d0b05fdbf9";
           UserHandler uh = new UserHandler();
           string s = uh.getUserHashedPW(15,"Ariel");
         //  Console.WriteLine(s);
           Assert.IsTrue(s.Length==result.Length);
           for (int i =0; i < s.Length; i++) {
               Assert.IsTrue(s[i]==result[i]);
           }
           Assert.IsTrue(s.Equals(result));
       }

   
    }
}