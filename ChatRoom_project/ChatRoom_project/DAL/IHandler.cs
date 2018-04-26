using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PersistentLayer
{
    interface IHandler<T>
    {
        /*
         * Saves data in the DB
         * Throws ArgumentNullException if data is null
         * Does nothing if data already exits in retriveAll()
         */
        void save(T data);
        void edit(T data);
        List<T> retriveAll();
    }
}
