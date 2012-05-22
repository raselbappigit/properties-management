using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;
using PropertyManage.Data;

namespace PropertyManage.Service
{
    public interface IUnitValueService
    {
        IEnumerable<UnitValue> GetUnitValues();

        UnitValue GetUnitValue(int id);
        void CreateUnitValue(UnitValue unitValue);
        void UpdateUnitValue(UnitValue unitValue);
        void DeleteUnitValue(int id);

        void Save();
    }

    public class UnitValueService : IUnitValueService
    {
        private readonly IUnitValueRepository _unitValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnitValueService(IUnitValueRepository unitValueRepository, IUnitOfWork unitOfWork)
        {
            this._unitValueRepository = unitValueRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<UnitValue> GetUnitValues()
        {
            var unitValues = _unitValueRepository.GetAll();
            return unitValues;
        }

        public UnitValue GetUnitValue(int id)
        {
            var unitValue = _unitValueRepository.GetById(id);
            return unitValue;
        }

        public void CreateUnitValue(UnitValue unitValue)
        {
            _unitValueRepository.Add(unitValue);
            Save();
        }

        public void UpdateUnitValue(UnitValue unitValue)
        {
            _unitValueRepository.Update(unitValue);
            Save();
        }

        public void DeleteUnitValue(int id)
        {
            var unitValue = GetUnitValue(id);
            _unitValueRepository.Delete(unitValue);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }

}
