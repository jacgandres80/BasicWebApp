using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWebServerSocket
{
    public class PortAvalaible
    {
        public PortAvalaible(int pPort, PORT_STATE pState, DateTime? pDate = null)
        {
            this.Port = pPort;
            this.State = pState;
            this.DateRegister = pDate;
        }

        public int Port {get;set;}

        public DateTime? DateRegister { get; set; }

        public PORT_STATE State { get; set; }
    }

    public enum PORT_STATE
    {
        Available,
        Conected,
        Exit
    }
}
