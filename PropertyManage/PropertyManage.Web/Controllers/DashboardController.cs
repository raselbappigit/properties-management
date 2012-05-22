using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManage.Web.Helpers;
using PropertyManage.Web.Helpers;

namespace PropertyManage.Web.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            this.ShowTab();
            this.ShowTitle("Properties Management Application");
            this.ShowBreadcrumb("Dashboard", "Index");

            return View();
        }

    }
}
