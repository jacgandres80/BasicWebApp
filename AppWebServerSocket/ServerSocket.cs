using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Comon.XmlUtilities;
using Newtonsoft.Json;
using Server.Models;

namespace AppWebServerSocket
{
    public class ServerSocket
    {
        public static SocketState State { get; set; }
        public static string data = null;

        private IPHostEntry ipHostInfo;
        private IPAddress ipAddress;
        private IPEndPoint localEndPoint;
        private Socket listener;
        private Socket handler;

        public void StartListening(object pPort)
        {
            ServerSocket.State = SocketState.Waiting;
            int port = (int)pPort;

            byte[] bytes = new Byte[1024];

            ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[ipHostInfo.AddressList.Length - 1];
            localEndPoint = new IPEndPoint(ipAddress, port);


            listener = new Socket(AddressFamily.InterNetwork,
              SocketType.Stream, ProtocolType.Tcp);


            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    Console.WriteLine("Open Socket comunication: " + ipAddress.ToString() + ":" + port.ToString());
                    handler = listener.Accept();
                    ServerSocket.State = SocketState.Conected;
                    Console.WriteLine("Acepted Socket comunication: " + handler.LocalEndPoint.AddressFamily.ToString());
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

                    data = JsonConvert.SerializeObject(this.RegisterServer(JsonConvert.DeserializeObject<Register>(data), ipAddress.ToString(), port));

                    byte[] msg = Encoding.ASCII.GetBytes(data + "  <EOF>  ");
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    Console.WriteLine("Socket comunication: " + ipAddress.ToString() + ":" + port.ToString() + " has been closed");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket comunication: " + ipAddress.ToString() + ":" + port.ToString() + " has rised an error:" + e.Message);
                ServerSocket.State = SocketState.Exit;
            }
        }

        private Register RegisterServer(Register register, string ip, int port)
        {
            register.DateRegister = DateTime.Now;
            register.State = REGISTER_STATE.Conected;
            register.ServerAddress = ip;
            register.ServerWebAppPort = port;
            register.ActionSumServer = ACTION_SUM_SERVER.Disconected;

            List<Register> lstRegister = new List<Register>();
            lstRegister = XmlOptions.ObjectUnSerialize<List<Register>>(XmlFileOperation.ReadXmlContent(ConfigurationManager.AppSettings["LogPath"]));

            if (register.ActionRegister == ACTION_REGISTER.Register)
            {
                register.IdRegister = this.GetIdRegister(lstRegister);
                lstRegister.Add(register);
            }
            else
            {
                if (lstRegister.Remove(register) == false)
                {
                    if (lstRegister.RemoveAll(reg => reg.IdRegister == register.IdRegister) > 0)
                        register.State = REGISTER_STATE.Close;
                    else
                        register.State = REGISTER_STATE.Error;
                }
                else
                    register.State = REGISTER_STATE.Close;
            }

            XmlFileOperation.WriteXmlContent(XmlOptions.ObjectSerialize<List<Register>>(lstRegister).ToString(), ConfigurationManager.AppSettings["LogPath"]);

            return register;
        }

        private int GetIdRegister(List<Register> lstRegisters)
        {
            if (lstRegisters.Count > 0)
            {
                int id = ((from reg in lstRegisters
                           orderby reg.IdRegister descending
                           select reg).FirstOrDefault()).IdRegister;
                id = id + 1;
                return id;
            }
            else
                return 1;
        }
    }
    public enum SocketState
    {
        Conected,
        Exit,
        Waiting
    }
}
