using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManage.Domain;
using PropertyManage.Service;
using PropertyManage.Web.Helpers;
using PropertyManage.Domain;

namespace PropertyManage.Web.Controllers
{
    public class UnittypeController : Controller
    {
        private readonly IUnitTypeService _unitTypeService;

        public UnittypeController(IUnitTypeService unitTypeService)
        {
            this._unitTypeService = unitTypeService;
        }


        //
        // GET: /Unittype/

        public ActionResult Index()
        {
            return View();
        }

        // for display datatable
        public ActionResult GetUnitTypes(DataTableParamModel param)
        {
            var unitTypes = _unitTypeService.GetUnitTypes().ToList();

            var viewUnitTypes = unitTypes.Select(u => new UnitTypeTableModels() { UnitValueId = Convert.ToString(u.UnitTypeId), Name = u.Name });

            IEnumerable<UnitTypeTableModels> filteredUnitTypes;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredUnitTypes = viewUnitTypes.Where(usr => (usr.Name ?? "").Contains(param.sSearch)).ToList();
            }
            else
            {
                filteredUnitTypes = viewUnitTypes;
            }

            var viewOdjects = filteredUnitTypes.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from utMdl in viewOdjects
                         select new[] { utMdl.UnitValueId, utMdl.Name };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = unitTypes.Count(),
                iTotalDisplayRecords = filteredUnitTypes.Count(),
                aaData = result
            },
                            JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /Unit/Details/by id

        public ActionResult Details(int id)
        {
            this.ShowTitle("Setting Management");
            UnitType unitType = _unitTypeService.GetUnitType(id);

            if (unitType == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(unitType);
        }

        //
        // GET: /Unit/Create

        public ActionResult Create()
        {
            this.ShowTitle("Setting Management");
            return View();
        }

        //
        // POST: /Unit/Create by object

        [HttpPost]
        public ActionResult Create(UnitType model)
        {
            this.ShowTitle("Setting Management");
            if (ModelState.IsValid)
            {
                try
                {
                    _unitTypeService.CreateUnitType(model);
                    this.ShowMessage("Unit type created successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }
            }

            return View(model);
        }

        //
        // GET: /Unit/Edit/by id

        public ActionResult Edit(int id)
        {
            this.ShowTitle("Setting Management");
            UnitType unitType = _unitTypeService.GetUnitType(id);

            if (unitType == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(unitType);
        }

        //
        // POST: /Unit/Edit/by object

        [HttpPost]
        public ActionResult Edit(UnitType model)
        {
            this.ShowTitle("Setting Management");
            if (ModelState.IsValid)
            {
                UnitType unitType = _unitTypeService.GetUnitType(model.UnitTypeId);

                if (unitType == null)
                {
                    this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                    return RedirectToAction("Index");
                }

                try
                {
                    unitType.Name = model.Name;
                    _unitTypeService.UpdateUnitType(unitType);
                    this.ShowMessage("Unit type updated successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }


            }
            return View(model);
        }

        //
        // GET: /Unit/Delete/by id

        public ActionResult Delete(int id)
        {
            this.ShowTitle("Setting Management");
            UnitType unitType = _unitTypeService.GetUnitType(id);

            if (unitType == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(unitType);
        }

        //
        // POST: /Unit/Delete/by id

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitType unitType = _unitTypeService.GetUnitType(id);

            if (unitType == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }

            try
            {
                _unitTypeService.DeleteUnitType(unitType.UnitTypeId);
                this.ShowMessage("Unit type deleted successfully", MessageType.Success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
            }

            return View(unitType);
        }

    }

}