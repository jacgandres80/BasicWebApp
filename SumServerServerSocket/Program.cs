using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumServerServerSocket
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServerSocket sock = new ServerSocket();
            sock.StartListening(int.Parse(args[0]));
        }
    }
}
