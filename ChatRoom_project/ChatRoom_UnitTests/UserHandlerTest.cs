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
    public class UserHandlerTest
    {
        private static readonly string dirPath =
            System.IO.Directory.GetCurrentDirectory() + "\\local_files";
        //Tests save method
        [TestMethod]
        public void SaveUserTest()
        {
            UserHandler uh = new UserHandler();
            string filesPath = uh.getPath();
            User u = new User(123, "456");
            List<User> data = getData(filesPath);
            //The user should not exist in the files system
            if (data != null)
            {
                Assert.IsFalse(data.Contains(u), "User shoud'nt be in DB before test");
            }

            uh.save(u);

            //The user should exist in the files system
            data = getData(filesPath);
            Assert.IsNotNull(data, "Data should be initialized");
            Assert.IsTrue(data.Contains(u), "User shpuld be in DB");
            Assert.IsTrue(data.Count == 1, "User shpuld be only in db");

        }
        [TestMethod]
        public void EditUserTest()
        {
            List<User> data;
            UserHandler uh = new UserHandler();
            string filesPath = uh.getPath();
            User u = new User(123, "456");
            uh.save(u);
            /*
             * No details to edit in user so that it can equals
             * itself after editing
             */
        }
        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("Cleaning");
            System.IO.DirectoryInfo di = new DirectoryInfo(dirPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        public static List<User> getData(string filesPath)
        {
            List<User> ans = null;
            if (File.Exists(filesPath))
            {
                Stream myOtherFileStream = File.OpenRead(filesPath);
                BinaryFormatter deserializer = new BinaryFormatter();
                ans = (List<User>)deserializer.Deserialize(myOtherFileStream);
                myOtherFileStream.Close();
            }
            return ans;
        }
    }
}
