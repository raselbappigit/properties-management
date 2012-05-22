using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;
using PropertyManage.Data;

namespace PropertyManage.Service
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();

        Product GetProduct(int id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);

        void Save();
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = _productRepository.GetAll();
            return products;
        }

        public Product GetProduct(int id)
        {
            var product = _productRepository.GetById(id);
            return product;
        }

        public void CreateProduct(Product product)
        {
            _productRepository.Add(product);
            Save();
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Update(product);
            Save();
        }

        public void DeleteProduct(int id)
        {
            var product = GetProduct(id);
            _productRepository.Delete(product);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }

}
