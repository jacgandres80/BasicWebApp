using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SumServerServerSocket
{
    public class ServerSocket
    {
        public string data = null;

        public void StartListening(int port)
        {
            byte[] bytes = new Byte[1024];

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[ipHostInfo.AddressList.Length - 1];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting Socket comunication: " + ipAddress.ToString() + ":" + port.ToString());
                    Socket handler = listener.Accept();
                    Console.WriteLine("Open Socket comunication: " + ipAddress.ToString() + ":" + port.ToString());

                    data = null;

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("  <EOF>  ") > -1)
                            break;
                    }

                    data = data.Replace("  <EOF>  ", string.Empty);

                    List<double> numbers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<double>>(data);
                    double result = this.SumValues(numbers);

                    string strMessage = Newtonsoft.Json.JsonConvert.SerializeObject(result) + "  <EOF>  ";

                    byte[] msg = Encoding.ASCII.GetBytes(strMessage);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                    Console.WriteLine("Closing Socket comunication: " + ipAddress.ToString() + ":" + port.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
        }

        private double SumValues(List<double> pNumbers)
        {
            using (SumServerServerSocket.SumService.ServerSumOperationServiceClient service = new SumServerServerSocket.SumService.ServerSumOperationServiceClient())
            {
                return service.SumNumbers(pNumbers);
            }
        }
    }
}
