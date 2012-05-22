using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManage.Service;
using PropertyManage.Web.Helpers;

namespace PropertyManage.Web.Controllers
{
    //[Authorize]
    //[System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class SettingController : Controller
    {
        public ActionResult Index()
        {
            this.ShowTab();
            this.ShowTitle("Setting Management");
            this.ShowBreadcrumb("Setting", "Index");
            return View();
        }

    }
}