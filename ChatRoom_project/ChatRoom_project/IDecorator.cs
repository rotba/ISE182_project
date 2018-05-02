using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom_project
{
    interface IDecorator<T>
    {
        List<T> decorate(List<T> list);
    }
}
