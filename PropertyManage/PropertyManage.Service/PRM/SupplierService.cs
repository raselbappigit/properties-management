using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;
using PropertyManage.Data;

namespace PropertyManage.Service
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetSuppliers();

        Supplier GetSupplier(int id);
        void CreateSupplier(Supplier supplier);
        void UpdateSupplier(Supplier supplier);
        void DeleteSupplier(int id);

        void Save();
    }

    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork)
        {
            this._supplierRepository = supplierRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            var suppliers = _supplierRepository.GetAll();
            return suppliers;
        }

        public Supplier GetSupplier(int id)
        {
            var supplier = _supplierRepository.GetById(id);
            return supplier;
        }

        public void CreateSupplier(Supplier supplier)
        {
            _supplierRepository.Add(supplier);
            Save();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            _supplierRepository.Update(supplier);
            Save();
        }

        public void DeleteSupplier(int id)
        {
            var supplier = GetSupplier(id);
            _supplierRepository.Delete(supplier);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }

}
