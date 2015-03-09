using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Comon.XmlUtilities;
using Newtonsoft.Json;
using Server.Models;
using SumServer.Attributes;

namespace AppWebServer.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        private Register GetConectInfo(List<Register> lstServer, ACTION_SUM_SERVER ActionCondicional, ACTION_SUM_SERVER ActionState, string path)
        {
            foreach (Register reg in lstServer)
            {
                if (reg.ActionSumServer == ActionCondicional)
                {
                    reg.ActionSumServer = ActionState;
                    lstServer.RemoveAll(r => r.IdRegister == reg.IdRegister);
                    lstServer.Add(reg);
                    Common.XmlFileOperation.WriteXmlContent(XmlOptions.ObjectSerialize<List<Register>>(lstServer).ToString(), path);
                    return reg;
                }
            }
            return null;
        }

        [HttpPost]
        [OnlyAjaxRequestAttribute]
        public JsonResult GetSumValues(bool pAutomatic, List<double> pNumbers, DateTime pDate)
        {
            string path = System.Web.HttpContext.Current.Session["XmlPathFile"].ToString();
            System.Threading.Thread.Sleep(5000);
            List<Register> lstRegisters = new List<Register>();
            string xml = Common.XmlFileOperation.ReadXmlContent(path);
            lstRegisters = XmlOptions.ObjectUnSerialize<List<Register>>(xml);
            Register tmpRegister = this.GetConectInfo(lstRegisters, ACTION_SUM_SERVER.Disconected, ACTION_SUM_SERVER.Conected, path);
            if (tmpRegister != null)
            {
                if (lstRegisters.Count > 0)
                {
                    if (pAutomatic == false)
                    {
                        pNumbers = new List<double>();
                        Random rnd = new Random(5);
                        for (int i = 0; i < 25; i++)
                            pNumbers.Add(rnd.NextDouble());
                    }
                    double result = 0d;
                    AppWebClientSocket.SocketClient sc = new AppWebClientSocket.SocketClient();
                    result = sc.StartClient(tmpRegister.SumServerAddress, tmpRegister.SumServerPort, pNumbers);
                    this.GetConectInfo(lstRegisters, ACTION_SUM_SERVER.Conected, ACTION_SUM_SERVER.Disconected, path);

                    return Json(new { RegisteredServers = lstRegisters, dateEnd = (DateTime.Now - pDate).Seconds, Value = result });
                }
                else
                    throw new ApplicationException("None server has been signed into the system yet..");
            }
            else
                return Json(new { Message = "The Registered Server are bussy, please try latter." });

        }
    }
}