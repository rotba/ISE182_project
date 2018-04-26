using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MileStoneClient.CommunicationLayer;
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
    public class RequestTests
    {
        public string Url { get => url; private set => url = value; }
        private static readonly string DEFAULT_URL = "http://ise172.ise.bgu.ac.il"; // project server url.
        private static string url=DEFAULT_URL;
        private string dirPath;
        private User tempUser;
        private Request request;
        private Request request2;

        [TestInitialize]
        public void Initialize() {
        request = new Request(url);
        request2 = new Request(url);
        tempUser = new User(15, "Ariel");
            dirPath =
                System.IO.Directory.GetCurrentDirectory() + "\\local_files";
        }
        [TestMethod()]
        public void RequestTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void sendTest()
        {
            IMessage imsg = request.send("Test", tempUser);
            Assert.IsTrue(imsg.MessageContent == "Test", "Content should be the same");
            Assert.IsTrue(imsg.GroupID == "15", "Group ID should be '15'");
            try
            {
                string g = "";
                for (int i = 0; i <= 150; i++)
                {
                    g = g + i;
                }
                request.send(g, tempUser);
                Assert.Fail();
            }
            catch (ToUserException e) { }
            try
            {
                for (int i = 0; i < 21; i++)
                {
                    request.send(" "+i+" ",tempUser);
                    System.Threading.Thread.Sleep(450);
                }
                Assert.Fail();
            }
            catch (ToUserException e) { }                                  
        }

        [TestMethod()]
        public void retrieveMessagesTest()
        {
            try
            {
                request.retrieveMessages(7);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {   }
            try
            {
                for (int i = 0; i < 21; i++)
                {
                    request.retrieveMessages(10);
                    System.Threading.Thread.Sleep(450);

                }
                Assert.Fail();
            }
            catch (ToUserException e) { }
         }
        [TestCleanup]
        public void Cleanup()
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}