using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ConsoleApp1.BuissnessLayer;
using ConsoleApp1.PersistentLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleApp1.UnitTest
{
    [TestClass]
    public class MessageHandlerTest
    {
        private static readonly string dirPath =
            System.IO.Directory.GetCurrentDirectory() + "\\local_files";
        private Guid id = Guid.NewGuid();
        //Tests save method
        [TestMethod]
        public void SaveMessageTest()
        {
            string m_user = "rotem";
            DateTime m_dt = DateTime.Now;
            string m_cont = "hello world";
            string m_gid = "15";
            MessageHandler mh = new MessageHandler();
            string filesPath = mh.getPath();
            Message m = new Message(id, m_user, m_dt, m_cont, m_gid);
            List<Message> data = getData(filesPath);
            //The user should not exist in the files system
            if (data != null)
            {
                Assert.IsFalse(data.Contains(m), "Message shoud'nt be in DB before test");
            }

            mh.save(m);

            //The user should exist in the files system
            data = getData(filesPath);
            Assert.IsNotNull(data, "Data should be initialized");
            Assert.IsTrue(data.Contains(m), "Message should be in the DB");
            Assert.IsTrue(data.Count == 1, "Message should be the only element in the DB");
            Assert.IsTrue(data[0].Id.Equals(id), "Message id wasn't preserved");
            Assert.IsTrue(data[0].Date.Equals(m_dt), "Message date wasn't preserved");
            Assert.IsTrue(data[0].UserName.Equals(m_user), "Message user details wasn't preserved");
            Assert.IsTrue(data[0].GroupID.Equals(m_gid), "Message Group id details wasn't preserved");
            Assert.IsTrue(data[0].MessageContent.Equals(m_cont), "Message content wasn't preserved");
        }
        [TestMethod]
        public void EditMessageTest()
        {
            /*
             * No details to edit in user so that it can equals
             * itself after editing
             */
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
        public static List<Message> getData(string filesPath)
        {
            List<Message> ans = null;
            if (File.Exists(filesPath))
            {
                Stream myOtherFileStream = File.OpenRead(filesPath);
                BinaryFormatter deserializer = new BinaryFormatter();
                ans = (List<Message>)deserializer.Deserialize(myOtherFileStream);
                myOtherFileStream.Close();
            }
            return ans;
        }
    }
}
