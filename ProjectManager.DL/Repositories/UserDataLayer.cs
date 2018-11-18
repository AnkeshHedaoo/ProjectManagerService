using ProjectManager.DL.EntityDataModel;
using ProjectManagerEntity;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.DL
{
    public class UserDataLayer : IUserDataLayer
    {
        ProjectManagerSQLConn _db;
        public UserDataLayer()
        {
            _db = new ProjectManagerSQLConn();
        }
        public List<UserEntity> GetAllUsers()
        {
            var usersFromDb = (from user in _db.T_USER
                               select new UserEntity
                               {
                                   EmployeeId = user.EMP_ID,
                                   FirstName = user.EMP_FRST_NM,
                                   LastName = user.EMP_LST_NM
                               }).ToList();

            return usersFromDb;
        }
        public void AddUser(UserEntity user)
        {
            var newUser = new T_USER();

            newUser.EMP_ID = user.EmployeeId;
            newUser.EMP_FRST_NM = user.FirstName;
            newUser.EMP_LST_NM = user.LastName;

            _db.T_USER.Add(newUser);
            _db.SaveChanges();
        }
        public void DeleteUser(int employeeId)
        {
            var userFromDb = (from u in _db.T_USER
                              where u.EMP_ID == employeeId
                              select u).FirstOrDefault();

            _db.T_USER.Remove(userFromDb);
            _db.SaveChanges();
        }
        public void UpdateUser(UserEntity user)
        {
            var userFromDb = (from u in _db.T_USER
                              where u.EMP_ID == user.EmployeeId
                              select u).FirstOrDefault();

            userFromDb.EMP_FRST_NM = user.FirstName;
            userFromDb.EMP_LST_NM = user.LastName;

            _db.SaveChanges();
        }
    }
}