using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManage.Service;
using PropertyManage.Web.Helpers;

namespace PropertyManage.Web.Controllers
{
    //[Authorize]
    //[System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PurchaseController : Controller
    {
        public ActionResult Index()
        {
            this.ShowTab();
            this.ShowTitle("Purchase Management");
            this.ShowBreadcrumb("Purchase", "Index");
            return View();
        }
    }
}
