using ProjectManagerEntity;
using System.Collections.Generic;

namespace ProjectManager.DL
{
    public interface IUserDataLayer
    {
        List<UserEntity> GetAllUsers();
        void AddUser(UserEntity user);
        void UpdateUser(UserEntity user);
        void DeleteUser(int employeeId);
    }
}
