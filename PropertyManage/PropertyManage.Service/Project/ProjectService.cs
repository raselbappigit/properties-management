using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Data;
using PropertyManage.Domain;

namespace PropertyManage.Service
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjects();

        Project GetProject(int id);
        void CreateProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(int id);

        void Save();
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            this._projectRepository = projectRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Project> GetProjects()
        {
            var projects = _projectRepository.GetAll();
            return projects;
        }

        public Project GetProject(int id)
        {
            var project = _projectRepository.GetById(id);
            return project;
        }

        public void CreateProject(Project project)
        {
            _projectRepository.Add(project);
            Save();
        }

        public void UpdateProject(Project project)
        {
            _projectRepository.Update(project);
            Save();
        }

        public void DeleteProject(int id)
        {
            var project = GetProject(id);
            _projectRepository.Delete(project);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }

}
