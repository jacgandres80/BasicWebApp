using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Server.Models;
using SumServer.Attributes;
using SumServerClientSocket;

namespace SumServer.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [OnlyAjaxRequestAttribute]
        public JsonResult DoServerRegistration()
        {
            Register reg = new Register();
             
            reg.HostUrl = base.Request.Url.ToString();
            reg.SumServerName = ConfigurationManager.AppSettings["SumServerName"];
            reg.SumServerPort = int.Parse(ConfigurationManager.AppSettings["initialPortSumServer"]);
            reg.ServerWebAppPort = int.Parse(ConfigurationManager.AppSettings["initialPortWebAppSer"]);
            reg.HostName = base.Request.UserHostName;
            reg.State = REGISTER_STATE.Pending;
            reg.DateRegister = DateTime.Now;
            reg.ActionRegister = ACTION_REGISTER.Register;

            SocketClient sClient = new SocketClient();
            reg = sClient.StartClient(reg, reg.ServerWebAppPort);

            Session["Register"] = reg;
            return Json(new { Register = reg });
        }

        [HttpPost]
        [OnlyAjaxRequestAttribute]
        public JsonResult DoServerUnRegistration()
        {
            Register reg = (Register)Session["Register"];
            reg.ActionRegister = ACTION_REGISTER.Unregister;
            SocketClient sClient = new SocketClient();
            reg = sClient.StartClient(reg, reg.SumServerPort);
            Session.Remove("Register");
            return Json(new { Register = reg });
        }


    }
}