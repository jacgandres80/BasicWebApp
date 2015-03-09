using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Models;

namespace AppWebClientSocket
{
    public class SocketClient
    {
        public double StartClient(string IpAddresSumServer, int portSumServer, List<double> pNumbers)
        {
            byte[] bytes = new byte[1024];

            try
            { 
                IPHostEntry iphostInfo = Dns.GetHostEntry(IpAddresSumServer);
                IPAddress ipAddress = iphostInfo.AddressList[iphostInfo.AddressList.Length - 1];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, portSumServer);
                var memoryStream = new System.IO.MemoryStream();

                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                bool controller = true;
                double sum = 0d;
                while (controller)
                {
                    try
                    {
                        sender.Connect(remoteEP);

                        Console.WriteLine("Open Socket comunication: " + ipAddress.ToString() + ":" + portSumServer.ToString());

                        string strMessage = Newtonsoft.Json.JsonConvert.SerializeObject(pNumbers) + "  <EOF>  ";

                        byte[] msg = Encoding.ASCII.GetBytes(strMessage);

                        int bytesSent = sender.Send(msg);

                        int bytesRec = sender.Receive(bytes);

                        string result = Encoding.ASCII.GetString(bytes, 0, bytesRec).Replace("  <EOF>  ", string.Empty);

                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();
                        sum = Newtonsoft.Json.JsonConvert.DeserializeObject<double>(result);

                        controller = false;
                    }
                    catch (ArgumentNullException ane)
                    {
                        throw ane;
                    }
                    catch (SocketException)
                    {
                        portSumServer = portSumServer + 1;
                        remoteEP = new IPEndPoint(ipAddress, portSumServer);
                    }
                    catch (Exception e1)
                    {
                        throw e1;
                    }
                }
                return sum;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
