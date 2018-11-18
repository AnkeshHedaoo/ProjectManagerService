using ProjectManagerEntity;
using System.Collections.Generic;

namespace ProjectManager.BL
{
    public interface IUserBL
    {
        List<UserEntity> GetAllUsers();
        void AddUser(UserEntity user);
        void UpdateUser(UserEntity user);
        void DeleteUser(int employeeId);
    }
}
