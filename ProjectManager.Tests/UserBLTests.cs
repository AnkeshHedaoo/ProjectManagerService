using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ProjectManager.DL;
using ProjectManagerEntity;
using Moq;

namespace ProjectManager.Tests
{
    [TestFixture]
    public class UserBLTests
    {
        private IUserDataLayer _mockRepository;
        private List<UserEntity> _users;

        [SetUp]
        public void Initialize()
        {
            var repository = new Mock<IUserDataLayer>();
            _users = new List<UserEntity>()
                        {
                            new UserEntity { EmployeeId = 1234567, FirstName = "Test", LastName = "User1" },
                            new UserEntity { EmployeeId = 1234568, FirstName = "Testing", LastName = "User2" },
                            new UserEntity { EmployeeId = 1234569, FirstName = "Tested", LastName = "User3" },
                        };

            // Get All
            repository.Setup(r => r.GetAllUsers()).Returns(_users);

            // Insert User
            repository.Setup(r => r.AddUser(It.IsAny<UserEntity>()))
                .Callback((UserEntity u) => _users.Add(u));

            // Update User
            repository.Setup(r => r.UpdateUser(It.IsAny<UserEntity>())).Callback(
                (UserEntity target) =>
                {
                    var original = _users.Where(
                        q => q.EmployeeId == target.EmployeeId).Single();

                    original.FirstName = target.FirstName;
                    original.LastName = target.LastName;
                });

            // Delete User
            repository.Setup(r => r.DeleteUser(It.IsAny<int>()))
                .Callback((int employeeId) => _users.Remove(GetUserById(employeeId)));

            _mockRepository = repository.Object;
        }

        [Test]
        public void Get_All_Users()
        {
            List<UserEntity> users = _mockRepository.GetAllUsers();

            Assert.IsTrue(users.Count() == 3);
            Assert.IsTrue(users.ElementAt(1).FirstName == "Testing");
            Assert.IsTrue(users.ElementAt(0).LastName == "User1");
            Assert.IsTrue(users.ElementAt(2).EmployeeId == 1234569);
            Assert.IsTrue(users.ElementAt(0).FirstName == "Test");
        }

        [Test]
        public void Add_User()
        {
            var employeeId = 1234570;
            var user = new UserEntity
            {
                EmployeeId = employeeId,
                FirstName = "Test",
                LastName = "User4"
            };

            _mockRepository.AddUser(user);
            Assert.IsTrue(_users.Count() == 4);
            UserEntity testUser = GetUserById(employeeId);
            Assert.IsNotNull(testUser);
            Assert.AreSame(testUser.GetType(), typeof(UserEntity));
            Assert.AreEqual(user.FirstName, testUser.FirstName);
        }

        [Test]
        public void Update_User()
        {
            var employeeId = 1234569;
            var user = new UserEntity
            {
                EmployeeId = employeeId,
                FirstName = "Testing",
                LastName = "User3"
            };

            _mockRepository.UpdateUser(user);

            var updatedUser = GetUserById(employeeId);
            Assert.IsTrue(updatedUser.FirstName == "Testing");
            Assert.IsTrue(updatedUser.LastName == "User3");
        }

        [Test]
        public void Delete_User()
        {
            var employeeId = 1234570;

            _mockRepository.DeleteUser(employeeId);

            var deletedUser = GetUserById(employeeId);
            Assert.IsTrue(deletedUser == null);
        }

        [TearDown]
        public void CleanUp()
        {
            _users.Clear();
        }

        private UserEntity GetUserById(int employeeId)
        {
            return _users.Where(x => x.EmployeeId == employeeId).Select(y => y).SingleOrDefault();
        }
    }
}
