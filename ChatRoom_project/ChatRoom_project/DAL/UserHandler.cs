using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private SqlConnection connection;
        private SqlCommand command;
        /// 
        /*
         *local
         */
        string connetion_string = null;
        string sql_query = null;
        string server_address = "localhost\\sqlexpress";
        string database_name = "MS3";
        /*
         *default
        string connetion_string = null;
        string sql_query = null;
        string server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
        string database_name = "db_lab";
        string user_name = "db_lab_user";
        string password = "hackMePlease";
         */
        /// 
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
        public bool checkIfExists(int g_id, string nickname) {
            bool ans = false;

            //defualt
            //connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";

            //local
            connetion_string = $"Server= {server_address}; Database= {database_name}; Integrated Security=True;";

            connection = new SqlConnection(connetion_string);
            SqlDataReader data_reader;

            try
            {
                connection.Open();
                Console.WriteLine("connected to: " + server_address);
                sql_query = $"SELECT * FROM USERS WHERE Group_Id={g_id} AND Nickname='{nickname}'" ;
                command = new SqlCommand(sql_query, connection);
                data_reader = command.ExecuteReader();
                ans = data_reader.Read();
                data_reader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            return ans;
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
