using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;
using PropertyManage.Data;

namespace PropertyManage.Service
{
    public interface IUnitTypeService
    {
        IEnumerable<UnitType> GetUnitTypes();

        UnitType GetUnitType(int id);
        void CreateUnitType(UnitType unitType);
        void UpdateUnitType(UnitType unitType);
        void DeleteUnitType(int id);

        void Save();
    }

    public class UnitTypeService : IUnitTypeService
    {
        private readonly IUnitTypeRepository _unitTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnitTypeService(IUnitTypeRepository unitTypeRepository, IUnitOfWork unitOfWork)
        {
            this._unitTypeRepository = unitTypeRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<UnitType> GetUnitTypes()
        {
            var unitTypes = _unitTypeRepository.GetAll();
            return unitTypes;
        }

        public UnitType GetUnitType(int id)
        {
            var unitType = _unitTypeRepository.GetById(id);
            return unitType;
        }

        public void CreateUnitType(UnitType unitType)
        {
            _unitTypeRepository.Add(unitType);
            Save();
        }

        public void UpdateUnitType(UnitType unitType)
        {
            _unitTypeRepository.Update(unitType);
            Save();
        }

        public void DeleteUnitType(int id)
        {
            var unitType = GetUnitType(id);
            _unitTypeRepository.Delete(unitType);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }

}
