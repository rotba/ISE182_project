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
    public class MessageHandler: IHandler<Message>
    {
        
        private List<Message> messages = new List<Message>();
        private readonly string filesPath =
            System.IO.Directory.GetCurrentDirectory() + "\\local_files\\messages.bin";

        //Initialize the the handler. Connects to the existing DB or 
        //creates one in a static directory
        public MessageHandler()
        {
            List<User> tmp;
            bool createdSuccefully = false;
            if (File.Exists(filesPath))
            {
                Stream myOtherFileStream = File.OpenRead(filesPath);
                BinaryFormatter deserializer = new BinaryFormatter();
                try
                {
                    messages = (List<Message>)deserializer.Deserialize(myOtherFileStream);
                    myOtherFileStream.Close();
                    createdSuccefully = true;
                }
                catch (SerializationException e)
                {
                    File.Delete(filesPath);
                    createFile();
                    throw e;
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
            serializer.Serialize(myFileStream, messages);
            myFileStream.Close();
        }
        //For test purposes
        public string getPath() {
            return filesPath;
        }


        /*
         * Saves data in the DB
         * Throws exception if data is null
         */
        public void edit(Message data)
        {
            List<Message> tmp = retriveAll();
            if (data == null)
                throw new ArgumentNullException("Edit null data request");
            if (!tmp.Contains(data)) return;
            tmp.Remove(data);
            save(data);
        }
        //Retrieves all the messages from the DB 
        public List<Message> retriveAll()
        {
            List<Message> ans = new List<Message>();
            foreach (Message m in messages)
            {
                ans.Add(new Message(m));
            }
            return ans;
        }

        //Stroes data in the DB
        public void save(Message data)
        {
            if (data == null)
                throw new ArgumentNullException("Save null data request");
            if (messages.Contains(data)) return;
            messages.Add(data);
            Stream myFileStream = File.Create(filesPath);
            BinaryFormatter serializes = new BinaryFormatter();
            serializes.Serialize(myFileStream, messages);
            myFileStream.Close();
        }
    }
}

