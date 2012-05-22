using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;
using PropertyManage.Data;

namespace PropertyManage.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategorys();

        Category GetCategory(int id);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);

        void Save();
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this._categoryRepository = categoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetCategorys()
        {
            var categorys = _categoryRepository.GetAll();
            return categorys;
        }

        public Category GetCategory(int id)
        {
            var category = _categoryRepository.GetById(id);
            return category;
        }

        public void CreateCategory(Category category)
        {
            _categoryRepository.Add(category);
            Save();
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepository.Update(category);
            Save();
        }

        public void DeleteCategory(int id)
        {
            var category = GetCategory(id);
            _categoryRepository.Delete(category);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }

}
