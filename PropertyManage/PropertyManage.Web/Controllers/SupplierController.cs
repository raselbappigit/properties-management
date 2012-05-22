using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManage.Domain;
using PropertyManage.Web.Helpers;
using PropertyManage.Service;


namespace PropertyManage.Web.Controllers
{
    //[Authorize]
    //[System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            this._supplierService = supplierService;
        }

        //
        // GET: /Supplier/

        public ActionResult Index()
        {
            this.ShowTitle("Supplier Management");
            this.ShowBreadcrumb("Supplier", "Index");
            return View();
        }


        // for display datatable
        public ActionResult GetSuppliers(DataTableParamModel param)
        {
            var suppliers = _supplierService.GetSuppliers().ToList();

            var viewSuppliers = suppliers.Select(s => new SupplierTableModels() { SupplierId = Convert.ToString(s.SupplierId), Name = s.Name, Address = s.Address, Mobile = s.Mobile, Email = s.Email, ContactPerson = s.ContactPerson });

            IEnumerable<SupplierTableModels> filteredSuppliers;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredSuppliers = viewSuppliers.Where(supp => (supp.Name ?? "").Contains(param.sSearch) || (supp.Mobile ?? "").Contains(param.sSearch) || (supp.Email ?? "").Contains(param.sSearch)).ToList();
            }
            else
            {
                filteredSuppliers = viewSuppliers;
            }

            var viewOdjects = filteredSuppliers.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from suppMdl in viewOdjects
                         select new[] { suppMdl.SupplierId, suppMdl.Name, suppMdl.Address, suppMdl.Mobile, suppMdl.Email, suppMdl.ContactPerson };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = suppliers.Count(),
                iTotalDisplayRecords = filteredSuppliers.Count(),
                aaData = result
            },
                            JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /Supplier/Details/by id

        public ActionResult Details(int id = 0)
        {
            this.ShowTitle("Supplier Management");
            this.ShowBreadcrumb("Supplier", "Details");

            Supplier supplier = _supplierService.GetSupplier(id);
            if (supplier == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Create

        public ActionResult Create()
        {
            this.ShowTitle("Supplier Management");
            this.ShowBreadcrumb("Supplier", "Create");
            return View();
        }

        //
        // POST: /Supplier/Create

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        _supplierService.CreateSupplier(supplier);
                        this.ShowMessage("Supplier created successfully", MessageType.Success);
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                    }
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }
            }

            return View(supplier);
        }

        //
        // GET: /Supplier/Edit/by id

        public ActionResult Edit(int id = 0)
        {
            this.ShowTitle("Supplier Management");
            this.ShowBreadcrumb("Supplier", "Edit");
            Supplier supplier = _supplierService.GetSupplier(id);
            if (supplier == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        //
        // POST: /Supplier/Edit/by odject

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    _supplierService.UpdateSupplier(supplier);
                    this.ShowMessage("Supplier updated successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }


            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Delete/by id

        public ActionResult Delete(int id = 0)
        {
            this.ShowTitle("Supplier Management");
            this.ShowBreadcrumb("Supplier", "Delete");
            Supplier supplier = _supplierService.GetSupplier(id);
            if (supplier == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        //
        // POST: /Supplier/Delete/by id

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = _supplierService.GetSupplier(id);

            if (supplier == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }

            try
            {
                _supplierService.DeleteSupplier(supplier.SupplierId);
                this.ShowMessage("Supplier deleted successfully", MessageType.Success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
            }
            return View(supplier);

        }

    }
}