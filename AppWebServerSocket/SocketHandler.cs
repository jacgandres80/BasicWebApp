using System;
using System.Collections.Generic;
using System.Threading;

namespace AppWebServerSocket
{
    public class SocketHandler
    {
        public bool ThreadController { get; set; }
        private int _initialPort;
        private PortAvalaible _SocketPort;
        private List<PortAvalaible> lstPorts;

        public PortAvalaible SocketPort
        {
            get
            {
                _SocketPort = GetPort();
                return _SocketPort;
            }
            set
            {
                SetPort(value);
            }
        }

        private void SetPort(PortAvalaible value)
        {
            if (lstPorts.Count == 0)
                lstPorts.Add(value);
            else if (lstPorts.Exists(prt => prt.Port == value.Port) == true)
            {
                PortAvalaible tmpPrt = lstPorts.Find(prt => prt.Port == value.Port);
                lstPorts.Remove(tmpPrt);
                lstPorts.Add(value);
            }
            else
                lstPorts.Add(value);

            _SocketPort = value;
        }

        private PortAvalaible GetPort()
        {
            if (lstPorts.Count == 0)
            {
                lstPorts.Add(new PortAvalaible(_initialPort, PORT_STATE.Available, null));
                return lstPorts[0];
            }
            else
            {
                for (int i = _initialPort; i < (_initialPort + 1000); i++)
                {
                    if (lstPorts.Exists(prt => prt.Port == i) != true)
                    {
                        lstPorts.Add(new PortAvalaible(i, PORT_STATE.Available, null));
                        return lstPorts.Find(prt => prt.Port == i);
                    }
                }

                throw new ApplicationException("Has occured and error when the application has been find it");
            }
        }

        public SocketHandler(int pInitialPort)
        {
            ThreadController = true; _initialPort = pInitialPort;
            lstPorts = new List<PortAvalaible>(); this.SocketPort = new PortAvalaible(_initialPort, PORT_STATE.Available, null);
            ServerSocket.State = SocketState.Conected; 
        }

        public void StartThread()
        {
            while (ThreadController)
            {
                if (ServerSocket.State == SocketState.Conected)
                { 
                    ServerSocket ss = new ServerSocket();
                    new Thread(ss.StartListening).Start(SocketPort.Port);
                    Thread.Sleep(1000);
                    SocketPort = new PortAvalaible(SocketPort.Port + 1, PORT_STATE.Available, null);
                }
                else if (ServerSocket.State == SocketState.Exit)
                    ThreadController = false;
                else
                    Thread.Sleep(1000);
            }
        }
    }
}
