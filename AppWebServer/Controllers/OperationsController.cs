using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SumServer.Attributes;

namespace AppWebServer.Controllers
{
    public class OperationsController : Controller
    {
        // GET: Operations
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [OnlyAjaxRequestAttribute]
        public void StartSocket()
        { 

        }
    }
}