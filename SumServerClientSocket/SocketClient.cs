using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Server.Models;

namespace SumServerClientSocket
{
    public class SocketClient
    {
        public Register StartClient(Register pRegister, int pInitialPort)
        {
            byte[] bytes = new byte[1024];
            int portSumServer = pInitialPort;
            try
            {
                IPHostEntry iphostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = iphostInfo.AddressList[iphostInfo.AddressList.Length - 1];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, portSumServer);

                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                bool controllerPort = true;

                while (controllerPort)
                {
                    try
                    {
                        sender.Connect(remoteEP);

                        pRegister.SumServerAddress = ipAddress.ToString();

                        byte[] msg = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(pRegister) + "  <EOF>  ");

                        // Send the data through the socket.
                        int bytesSent = sender.Send(msg);
                        string data = string.Empty;
                        // Receive the response from the remote device.
                        while (true)
                        {
                            bytes = new byte[1024];
                            int bytesRec = sender.Receive(bytes);
                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            if (data.IndexOf("  <EOF>  ") > -1)
                                break;
                        }
                        data = data.Replace("  <EOF>  ", string.Empty);
                        pRegister = JsonConvert.DeserializeObject<Register>(data);


                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();
                        controllerPort = false;
                        sender.Dispose();
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
                return pRegister;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
