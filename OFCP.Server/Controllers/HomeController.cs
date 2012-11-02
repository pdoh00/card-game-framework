using OFCP.Server.ActionFilterAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OFCP.Server.Controllers
{
    public class HomeController : Controller
    {
        [EnableCors]
        [AllowCrossSiteJson]
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        [EnableCors]
        [AllowCrossSiteJson]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [EnableCors]
        [AllowCrossSiteJson]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
