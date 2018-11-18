using ProjectManagerEntity;
using System.Collections.Generic;
using System;
using ProjectManager.DL;

namespace ProjectManager.BL
{
    public class UserBL : IUserBL
    {
        private IUserDataLayer _repo;

        public UserBL(IUserDataLayer repo)
        {
            _repo = repo;
        }
        public List<UserEntity> GetAllUsers()
        {
            return _repo.GetAllUsers();
        }

        public void AddUser(UserEntity user)
        {
            _repo.AddUser(user);
        }

        public void DeleteUser(int employeeId)
        {
            _repo.DeleteUser(employeeId);
        }

        public void UpdateUser(UserEntity user)
        {
            _repo.UpdateUser(user);
        }
    }
}
