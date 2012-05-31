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

namespace PropertyManage.Web.Controllers
{
    //[Authorize]
    //[System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View();
        }

        // for display datatable
        public ActionResult GetCategories(DataTableParamModel param)
        {
            var categorys = _categoryService.GetCategorys().ToList();

            var viewCategorys = categorys.Select(c => new CategoryTableModels() { CategoryId = Convert.ToString(c.CategoryId), Name = c.Name });

            IEnumerable<CategoryTableModels> filteredCategorys;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredCategorys = viewCategorys.Where(cat => (cat.Name ?? "").Contains(param.sSearch)).ToList();
            }
            else
            {
                filteredCategorys = viewCategorys;
            }

            var viewOdjects = filteredCategorys.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from catMdl in viewOdjects
                         select new[] { catMdl.CategoryId, catMdl.Name };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = categorys.Count(),
                iTotalDisplayRecords = filteredCategorys.Count(),
                aaData = result
            },
                            JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id = 0)
        {
            Category category = _categoryService.GetCategory(id);
            if (category == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.CreateCategory(category);
                    this.ShowMessage("Land type created successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }

            }

            return View(category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Category category = _categoryService.GetCategory(id);
            if (category == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // POST: /Category/Edit/by id

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.UpdateCategory(category);
                    this.ShowMessage("Land type updated successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }
                
            }
            return View(category);
        }

        //
        // GET: /Category/Delete/by id

        public ActionResult Delete(int id = 0)
        {
            Category category = _categoryService.GetCategory(id);
            if (category == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryService.GetCategory(id);

            if (category == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }

            try
            {
                _categoryService.DeleteCategory(category.CategoryId);
                this.ShowMessage("Land type deleted successfully", MessageType.Success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
            }
            return View(category);

        }

    }
}