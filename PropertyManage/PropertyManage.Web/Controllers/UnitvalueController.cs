using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManage.Service;
using PropertyManage.Web.Helpers;
using PropertyManage.Domain;

namespace PropertyManage.Web.Controllers
{
    //[Authorize]
    //[System.Web.Mvc.OutputCache(NoStore = true, Durtion = 0, VaryByParam = "*")]
    public class UnitvalueController : Controller
    {
        private readonly IUnitTypeService _unitTypeService;
        private readonly IUnitValueService _unitValueService;

        public UnitvalueController(IUnitTypeService unitTypeService, IUnitValueService unitValueService)
        {
            this._unitTypeService = unitTypeService;
            this._unitValueService = unitValueService;
        }

        //
        // GET: /Unit/

        public ActionResult Index()
        {
            this.ShowTitle("Setting Management");
            this.ShowBreadcrumb("Unit", "Index");
            return View();
        }

        // for display datatable
        public ActionResult GetUnitValues(DataTableParamModel param)
        {
            var unitValues = _unitValueService.GetUnitValues().ToList();

            var viewUnitValues = unitValues.Select(u => new UnitValueTableModels() { UnitValueId = Convert.ToString(u.UnitValueId), Name = u.Name, Note = u.Note, UnitTypeName = u.UnitType == null ? null : Convert.ToString(u.UnitType.Name) });

            IEnumerable<UnitValueTableModels> filteredUnitValues;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredUnitValues = viewUnitValues.Where(usr => (usr.Name ?? "").Contains(param.sSearch) || (usr.UnitTypeName ?? "").Contains(param.sSearch)).ToList();
            }
            else
            {
                filteredUnitValues = viewUnitValues;
            }

            var viewOdjects = filteredUnitValues.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from uvMdl in viewOdjects
                         select new[] { uvMdl.UnitValueId, uvMdl.Name, uvMdl.Note, uvMdl.UnitTypeName };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = unitValues.Count(),
                iTotalDisplayRecords = filteredUnitValues.Count(),
                aaData = result
            },
                            JsonRequestBehavior.AllowGet);
        }

        // Unit of value CRUD

        //
        // GET: /Unit/Details/by id

        public ActionResult Details(int id)
        {
            this.ShowTitle("Setting Management");

            UnitValue unitValue = _unitValueService.GetUnitValue(id);

            if (unitValue == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(unitValue);
        }

        //
        // GET: /Unit/Create

        public ActionResult Create()
        {
            this.ShowTitle("Setting Management");

            ViewBag.UnitTypeId = new SelectList(_unitTypeService.GetUnitTypes(), "UnitTypeId", "Name");

            return View();
        }

        //
        // POST: /Unit/CreateValue by object

        [HttpPost]
        public ActionResult Create(UnitValue model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitValueService.CreateUnitValue(model);
                    this.ShowMessage("Unit value created successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }

            }

            ViewBag.UnitTypeId = new SelectList(_unitTypeService.GetUnitTypes(), "UnitTypeId", "Name", model.UnitTypeId);
            return View(model);
        }

        //
        // GET: /Unit/Edit/by id

        public ActionResult Edit(int id)
        {
            this.ShowTitle("Setting Management");
            UnitValue unitValue = _unitValueService.GetUnitValue(id);

            if (unitValue == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }

            ViewBag.UnitTypeId = new SelectList(_unitTypeService.GetUnitTypes(), "UnitTypeId", "Name", unitValue.UnitTypeId);
            return View(unitValue);
        }

        //
        // POST: /Unit/Edit/by object

        [HttpPost]
        public ActionResult Edit(UnitValue model)
        {
            if (ModelState.IsValid)
            {
                UnitValue unitValue = _unitValueService.GetUnitValue(model.UnitValueId);
                if (unitValue == null)
                {
                    this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                    return RedirectToAction("Index");
                }

                try
                {
                    unitValue.Name = model.Name;
                    unitValue.Note = model.Note;
                    unitValue.UnitTypeId = model.UnitTypeId;
                    _unitValueService.UpdateUnitValue(unitValue);
                    this.ShowMessage("Unit value updated successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }

            }

            ViewBag.UnitTypeId = new SelectList(_unitTypeService.GetUnitTypes(), "UnitTypeId", "Name", model.UnitTypeId);
            return View(model);
        }

        //
        // GET: /Unit/Delete/by id

        public ActionResult Delete(int id)
        {
            this.ShowTitle("Setting Management");

            UnitValue unitValue = _unitValueService.GetUnitValue(id);

            if (unitValue == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(unitValue);
        }

        //
        // POST: /Unit/Delete/by id

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitValue unitValue = _unitValueService.GetUnitValue(id);

            if (unitValue == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }

            try
            {
                _unitValueService.DeleteUnitValue(unitValue.UnitTypeId);
                this.ShowMessage("Unit value deleted successfully", MessageType.Success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
            }
            return View(unitValue);
        }

    }
}