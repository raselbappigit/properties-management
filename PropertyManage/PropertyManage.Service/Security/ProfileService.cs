using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertyManage.Domain;
using PropertyManage.Data;

namespace PropertyManage.Service
{
    public interface IProfileService
    {
        IEnumerable<Profile> GetProfiles();

        Profile GetProfile(int id);
        void CreateProfile(Profile profile);
        void UpdateProfile(Profile profile);
        void DeleteProfile(int id);

        void Save();
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(IProfileRepository profileRepository, IUnitOfWork unitOfWork)
        {
            this._profileRepository = profileRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Profile> GetProfiles()
        {
            var profiles = _profileRepository.GetAll();
            return profiles;
        }

        public Profile GetProfile(int id)
        {
            var profile = _profileRepository.GetById(id);
            return profile;
        }

        public void CreateProfile(Profile profile)
        {
            _profileRepository.Add(profile);
            Save();
        }

        public void UpdateProfile(Profile profile)
        {
            _profileRepository.Update(profile);
            Save();
        }

        public void DeleteProfile(int id)
        {
            var profile = GetProfile(id);
            _profileRepository.Delete(profile);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }
}
