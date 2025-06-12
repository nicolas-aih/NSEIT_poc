using IIIBL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIRegistrationPortal2.Controllers
{
    public class DPRangeController : Controller
    {
        // GET: DPRange
        [HttpGet]
        [AuthorizeExt]
        public ActionResult DPRange()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }
    }
}