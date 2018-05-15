using System;
using ConsoleApp1.BuissnessLayer;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class MultyUserSendTest
    {
        private ChatRoom chtrm;
        private string dirPath;

        [TestInitialize]
        public void Initialize()
        {
            chtrm = new ChatRoom();
            dirPath =
                System.IO.Directory.GetCurrentDirectory() + "\\local_files";
        }

        [TestMethod()]
        public void sendingRequests()
        {
            try
            {
                int count = 1;
                for (int i = 3234; i < 3260; i++)
                {
                    String s = "a";
                    for (int j = 0; j < 3; j++)
                    {

                        if (count % 20 == 0)
                        {
                            System.Threading.Thread.Sleep(10000);
                            count++;
                        }
                        else
                        {
                            if (count % 10 == 0)
                            {
                                System.Threading.Thread.Sleep(2500);
                                chtrm.register(i, s);
                                chtrm.login(i, s);
                                chtrm.send("Test " + count);
                                s = s + 'a';
                                count++;
                            }
                            else
                            {
                                chtrm.register(i, s);
                                chtrm.login(i, s);
                                chtrm.send("Test " + count);
                                s = s + 'a';
                                count++;
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
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
