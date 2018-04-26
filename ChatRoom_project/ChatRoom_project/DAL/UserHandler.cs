using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PersistentLayer
{
    public class UserHandler : IHandler<User>
    {
        private List<User> users = new List<User>();
        private static readonly string filesPath=
            System.IO.Directory.GetCurrentDirectory() + "\\local_files\\users.bin";

        //Initialize the the handler. Connects to the existing DB or 
        //creates one in a static directory
        public UserHandler()
        {
            List<User> tmp;
            bool createdSuccefully = false;
            if (File.Exists(filesPath))
            {
                Stream myOtherFileStream = File.OpenRead(filesPath);
                BinaryFormatter deserializer = new BinaryFormatter();
                try
                {
                    users = (List<User>)deserializer.Deserialize(myOtherFileStream);
                    myOtherFileStream.Close();
                    createdSuccefully = true;
                }
                catch (SerializationException e)
                {
                    File.Delete(filesPath);
                    createFile();
                }
                
            }
            else
            {
                createFile();
            }
        }
        private void createFile()
        {
            if (!Directory.Exists(System.IO.Directory.GetCurrentDirectory() + "\\local_files"))
            {
                Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + "\\local_files");
            }
            Stream myFileStream = File.Create(filesPath);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(myFileStream, users);
            myFileStream.Close();
        }
        //For test purposes
        public string getPath() { return filesPath; }


        /*
         * Saves data in the DB
         * Throws exception if data is null
         */
        public void edit(User data)
        {
            List<User> tmp = retriveAll();
            if (data == null)
                throw new ArgumentNullException("Edit null data request");
            if (!tmp.Contains(data)) return;
            tmp.Remove(data);
            save(data);
        }
        //Retrieves all the users from the DB
        public List<User> retriveAll()
        {
            List<User> ans = new List<User>();
            foreach (User u in users)
            {
                ans.Add(new User(u));
            }
            return ans;
            
        }
        //Stores the user in the DB
        public void save(User data)
        {
            if(data==null)
                throw new ArgumentNullException("Save null data request");
            if (users.Contains(data)) return;
            users.Add(data);
            Stream myFileStream = File.Create(filesPath);
            BinaryFormatter serializes = new BinaryFormatter();
            serializes.Serialize(myFileStream, users);
            myFileStream.Close();
        }
    }
}
