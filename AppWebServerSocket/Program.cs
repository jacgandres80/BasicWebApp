using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWebServerSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketHandler sh= new SocketHandler(int.Parse(args[0]));
            sh.StartThread();
        }
    }
}
