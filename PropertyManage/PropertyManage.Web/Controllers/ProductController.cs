using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManage.Domain;
using PropertyManage.Web.Models;
using PropertyManage.Web.Helpers;
using PropertyManage.Service;

namespace PropertyManage.Web.Controllers
{
    //[Authorize]
    //[System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProjectService _projectService;

        public ProductController(IProductService productService, ICategoryService categoryService, IProjectService projectService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._projectService = projectService;
        }

        // GET: /Product/

        public ActionResult Index()
        {
            this.ShowTitle("Land Management");
            this.ShowBreadcrumb("Product", "Index");
            return View();
        }


        // for display datatable
        public ActionResult GetProducts(DataTableParamModel param)
        {
            var products = _productService.GetProducts().ToList();

            var viewProducts = products.Select(p => new ProductTableModels() { ProductId = Convert.ToString(p.ProductId), Name = p.Name, MainCost = Convert.ToString(p.MainCost), OtherCost = Convert.ToString(p.OtherCost), CategoryName = p.Category == null ? null : p.Category.Name, ProjectName = p.Project == null ? null : p.Project.Name, UnitValueName = p.Project == null ? null : Convert.ToString(p.Project.UnitValue.Name) });

            IEnumerable<ProductTableModels> filteredProducts;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredProducts = viewProducts.Where(prod => (prod.Name ?? "").Contains(param.sSearch) || (prod.MainCost ?? "").Contains(param.sSearch) || (prod.CategoryName ?? "").Contains(param.sSearch) || (prod.ProjectName ?? "").Contains(param.sSearch)).ToList();
            }
            else
            {
                filteredProducts = viewProducts;
            }

            var viewOdjects = filteredProducts.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from prodMdl in viewOdjects
                         select new[] { prodMdl.ProductId, prodMdl.Name, prodMdl.MainCost, prodMdl.OtherCost, prodMdl.CategoryName, prodMdl.ProjectName, prodMdl.UnitValueName };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = products.Count(),
                iTotalDisplayRecords = filteredProducts.Count(),
                aaData = result
            },
                            JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Product/Details/by id

        public ActionResult Details(int id = 0)
        {
            Product product = _productService.GetProduct(id);
            if (product == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryService.GetCategorys(), "CategoryId", "Name");
            ViewBag.ProjectId = new SelectList(_projectService.GetProjects(), "ProjectId", "Name");
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productService.CreateProduct(product);
                    this.ShowMessage("Land created successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }

            }

            ViewBag.CategoryId = new SelectList(_categoryService.GetCategorys(), "CategoryId", "Name");
            ViewBag.ProjectId = new SelectList(_projectService.GetProjects(), "ProjectId", "Name");
            return View(product);
        }

        //
        // GET: /Product/Edit/by id

        public ActionResult Edit(int id = 0)
        {
            Product product = _productService.GetProduct(id);
            if (product == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_categoryService.GetCategorys(), "CategoryId", "Name");
            ViewBag.ProjectId = new SelectList(_projectService.GetProjects(), "ProjectId", "Name");
            return View(product);
        }

        //
        // POST: /Product/Edit/by id

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productService.UpdateProduct(product);
                    this.ShowMessage("Land updated successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }
            }
            ViewBag.CategoryId = new SelectList(_categoryService.GetCategorys(), "CategoryId", "Name");
            ViewBag.ProjectId = new SelectList(_projectService.GetProjects(), "ProjectId", "Name");
            return View(product);
        }

        //
        // GET: /Product/Delete/by id

        public ActionResult Delete(int id = 0)
        {
            Product product = _productService.GetProduct(id);
            if (product == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/by id

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _productService.GetProduct(id);

            if (product == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }

            try
            {
                _productService.DeleteProduct(product.ProductId);
                this.ShowMessage("Land deleted successfully", MessageType.Success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
            }
            return View(product);

        }

    }
}