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
        [TestMethod()]
        public void checkIfExistsTest()
        {
            UserHandler uh = new UserHandler();
            Assert.IsTrue(uh.checkIfExists(15, "Ariel"));
        }
    }
}