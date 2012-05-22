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
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUnitValueService _unitValueService;

        public ProjectController(IProjectService projectService, IUnitValueService unitValueService)
        {
            this._projectService = projectService;
            this._unitValueService = unitValueService;
        }

        //
        // GET: /Project/

        public ActionResult Index()
        {
            this.ShowTitle("Project Management");
            this.ShowBreadcrumb("Project", "Index");
            return View();
        }

        // for display datatable
        public ActionResult GetProjects(DataTableParamModel param)
        {
            var projects = _projectService.GetProjects().ToList();

            var viewProjects = projects.Select(p => new ProjectTableModels() { ProjectId = Convert.ToString(p.ProjectId), Name = p.Name, Location = p.Location, Description = p.Description, EstimatedArea = p.EstimatedArea, UnitValueName = p.UnitValue == null ? null : Convert.ToString(p.UnitValue.Name), DateCreated = Convert.ToString(p.DateCreated) });

            IEnumerable<ProjectTableModels> filteredProjects;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredProjects = viewProjects.Where(proj => (proj.Name ?? "").Contains(param.sSearch) || (proj.UnitValueName ?? "").Contains(param.sSearch) || (proj.Location ?? "").Contains(param.sSearch)).ToList();
            }
            else
            {
                filteredProjects = viewProjects;
            }

            var viewOdjects = filteredProjects.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from projMdl in viewOdjects
                         select new[] { projMdl.ProjectId, projMdl.Name, projMdl.Location, projMdl.Description, projMdl.EstimatedArea, projMdl.UnitValueName, projMdl.DateCreated };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = projects.Count(),
                iTotalDisplayRecords = filteredProjects.Count(),
                aaData = result
            },
                            JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /Project/Details/by id

        public ActionResult Details(int id = 0)
        {
            this.ShowTitle("Project Management");
            this.ShowBreadcrumb("Project", "Details");
            Project project = _projectService.GetProject(id);

            if (project == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            this.ShowTitle("Project Management");
            this.ShowBreadcrumb("Project", "Create");
            ViewBag.UnitValueId = new SelectList(_unitValueService.GetUnitValues(), "UnitValueId", "Name");
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _projectService.CreateProject(project);
                    this.ShowMessage("Project created successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }
            }

            ViewBag.UnitValueId = new SelectList(_unitValueService.GetUnitValues(), "UnitValueId", "Name", project.UnitValueId);
            return View(project);
        }

        //
        // GET: /Project/Edit/by id

        public ActionResult Edit(int id = 0)
        {
            this.ShowTitle("Project Management");
            this.ShowBreadcrumb("Project", "Edit");
            Project project = _projectService.GetProject(id);

            if (project == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }

            ViewBag.UnitValueId = new SelectList(_unitValueService.GetUnitValues(), "UnitValueId", "Name", project.UnitValueId);
            return View(project);
        }

        //
        // POST: /Project/Edit/by object

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                Project tempProject = _projectService.GetProject(project.ProjectId);
                if (tempProject == null)
                {
                    this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                    return RedirectToAction("Index");
                }

                try
                {
                    tempProject.Name = project.Name;
                    tempProject.Location = project.Location;
                    tempProject.Description = project.Description;
                    tempProject.EstimatedArea = project.EstimatedArea;
                    tempProject.UnitValueId = project.UnitValueId;
                    _projectService.UpdateProject(tempProject);
                    this.ShowMessage("Project updated successfully", MessageType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
                }
            }

            ViewBag.UnitValueId = new SelectList(_unitValueService.GetUnitValues(), "UnitValueId", "Name", project.UnitValueId);
            return View(project);
        }

        //
        // GET: /Project/Delete/by id

        public ActionResult Delete(int id = 0)
        {
            this.ShowTitle("Project Management");
            this.ShowBreadcrumb("Project", "Delete");
            Project project = _projectService.GetProject(id);
            if (project == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        //
        // POST: /Project/Delete/by id

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = _projectService.GetProject(id);
            if (project == null)
            {
                this.ShowMessage("Sorry! Data not found. You've been redirected to the default page instead.", MessageType.Error);
                return RedirectToAction("Index");
            }

            try
            {
                _projectService.DeleteProject(project.ProjectId);
                this.ShowMessage("Project deleted successfully", MessageType.Success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error on data generation with the following details " + ex.Message, MessageType.Error);
            }
            return View(project);
        }
    }
}