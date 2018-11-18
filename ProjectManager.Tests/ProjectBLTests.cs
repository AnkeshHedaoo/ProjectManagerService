using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ProjectManager.DL;
using ProjectManagerEntity;
using Moq;

namespace ProjectManager.Tests
{
    [TestFixture]
    public class ProjectBLTests
    {
        private IProjectDataLayer _mockRepository;
        private List<ProjectEntity> _projects;

        [SetUp]
        public void Initialize()
        {
            var repository = new Mock<IProjectDataLayer>();
            _projects = new List<ProjectEntity>()
                        {
                            new ProjectEntity { ProjectId = 1, ProjectName = "Project1", TasksCount = 4, Completed = 6, StartDate = "10/01/2018", EndDate = "10/20/2018", Priority = 2, ProjectManagerId = 1235467, ProjectManagerFullName = "Test User1" },
                            new ProjectEntity { ProjectId = 2, ProjectName = "Project2", TasksCount = 5, Completed = 0, StartDate = "10/10/2018", EndDate = "10/25/2018", Priority = 6, ProjectManagerId = 1235468, ProjectManagerFullName = "Test User2" },
                            new ProjectEntity { ProjectId = 3, ProjectName = "Project3", TasksCount = 7, Completed = 3, StartDate = "11/01/2018", EndDate = "11/30/2018", Priority = 12, ProjectManagerId = 1235469, ProjectManagerFullName = "Test User3" }
                        };

            // Get All
            repository.Setup(r => r.GetAllProjects()).Returns(_projects);

            // Insert Project
            repository.Setup(r => r.AddProject(It.IsAny<ProjectEntity>()))
                .Callback((ProjectEntity p) => _projects.Add(p));

            // Update Project
            repository.Setup(r => r.UpdateProject(It.IsAny<ProjectEntity>())).Callback(
                (ProjectEntity target) =>
                {
                    var original = _projects.Where(
                        q => q.ProjectId == target.ProjectId).Single();

                    original.ProjectName = target.ProjectName;
                    original.Priority = target.Priority;
                    original.ProjectManagerId = target.ProjectManagerId;
                    original.StartDate = target.StartDate;
                    original.EndDate = target.EndDate;
                });

            // Delete Project
            repository.Setup(r => r.SuspendProject(It.IsAny<int>()))
                .Callback((int projectId) => _projects.Remove(GetProjectById(projectId)));

            _mockRepository = repository.Object;
        }

        [Test]
        public void Get_All_Projects()
        {
            List<ProjectEntity> projects = _mockRepository.GetAllProjects();

            Assert.IsTrue(projects.Count() == 3);
            Assert.IsTrue(projects.ElementAt(0).ProjectName == "Project1");
            Assert.IsTrue(projects.ElementAt(0).StartDate == "10/01/2018");
            Assert.IsTrue(projects.ElementAt(1).Priority == 6);
            Assert.IsTrue(projects.ElementAt(1).ProjectManagerFullName == "Test User2");
            Assert.IsTrue(projects.ElementAt(2).TasksCount == 7);
            Assert.IsTrue(projects.ElementAt(2).Completed == 3);
        }

        [Test]
        public void Add_Project()
        {
            var projectId = _projects.Count() + 1;
            var project = new ProjectEntity
            {
                ProjectId = projectId,
                ProjectName = "Project4",
                StartDate = "11/01/2018",
                EndDate = "11/31/2018",
                Priority = 18,
                ProjectManagerId = 1234567
            };

            _mockRepository.AddProject(project);
            Assert.IsTrue(_projects.Count() == 4);
            ProjectEntity testProject = GetProjectById(projectId);
            Assert.IsNotNull(testProject);
            Assert.AreSame(testProject.GetType(), typeof(ProjectEntity));
            Assert.AreEqual(project.ProjectName, testProject.ProjectName);
            Assert.AreEqual(project.StartDate, testProject.StartDate);
            Assert.AreEqual(project.Priority, testProject.Priority);
            Assert.AreEqual(project.ProjectManagerId, testProject.ProjectManagerId);
        }

        [Test]
        public void Update_Project()
        {
            var projectId = 2;
            var project = new ProjectEntity
            {
                ProjectId = projectId,
                ProjectName = "Project2",
                StartDate = "11/05/2018",
                EndDate = "11/18/2018",
                Priority = 12,
                ProjectManagerId = 1234567
            };

            _mockRepository.UpdateProject(project);

            var updatedProject = GetProjectById(projectId);           
            Assert.IsTrue(updatedProject.Priority == 12);
            Assert.IsTrue(updatedProject.ProjectManagerId == 1234567);
        }

        [Test]
        public void Suspend_Project()
        {
            var projectId = 3;

            _mockRepository.SuspendProject(projectId);

            var deletedProject = GetProjectById(projectId);
            Assert.IsNull(deletedProject);
        }

        [TearDown]
        public void CleanUp()
        {
            _projects.Clear();
        }

        private ProjectEntity GetProjectById(int projectId)
        {
            return _projects.Where(x => x.ProjectId == projectId).Select(y => y).SingleOrDefault();
        }
    }
}
