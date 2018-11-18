using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ProjectManager.DL;
using ProjectManagerEntity;
using Moq;

namespace ProjectManager.Tests
{
    [TestFixture]
    public class TaskBLTests
    {
        private ITaskDataLayer _mockRepository;
        private List<ParentTaskEntity> _parentTasks;
        private List<TaskEntity> _tasks;

        [SetUp]
        public void Initialize()
        {
            var repository = new Mock<ITaskDataLayer>();

            _parentTasks = new List<ParentTaskEntity>()
                        {
                            new ParentTaskEntity { TaskId = 1, TaskName = "Parent1" },
                            new ParentTaskEntity { TaskId = 2, TaskName = "Parent2" },
                            new ParentTaskEntity { TaskId = 3, TaskName = "Parent3" }
                        };

            _tasks = new List<TaskEntity>()
                        {
                            new TaskEntity { TaskId = 1, TaskName = "Task1", ParentId = 1, ParentName = "Parent1", Priority = 5,  StartDate = "10/01/2018", EndDate = "10/15/2018", ProjectId = 1, ProjectName = "Project1", UserId = 1234567, UserName = "Test User1", TaskStatus = "A" },
                            new TaskEntity { TaskId = 2, TaskName = "Task2", ParentId = 1, ParentName = "Parent1", Priority = 12, StartDate = "10/16/2018", EndDate = "10/30/2018", ProjectId = 1, ProjectName = "Project1", UserId = 1234568, UserName = "Test User2", TaskStatus = "A" },
                            new TaskEntity { TaskId = 3, TaskName = "Task3", ParentId = 2, ParentName = "Parent2", Priority = 6,  StartDate = "11/01/2018", EndDate = "11/15/2018", ProjectId = 1, ProjectName = "Project1", UserId = 1234569, UserName = "Test User3", TaskStatus = "C" },
                            new TaskEntity { TaskId = 4, TaskName = "Task4", ParentId = 2, ParentName = "Parent2", Priority = 10, StartDate = "11/16/2018", EndDate = "11/30/2018", ProjectId = 1, ProjectName = "Project1", UserId = 1234570, UserName = "Test User4", TaskStatus = "A" },
                            new TaskEntity { TaskId = 5, TaskName = "Task5", ParentId = 3, ParentName = "Parent3", Priority = 18, StartDate = "12/01/2018", EndDate = "12/15/2018", ProjectId = 2, ProjectName = "Project2", UserId = 1234571, UserName = "Test User5", TaskStatus = "A" }
                        };

            // Get Parent tasks
            repository.Setup(r => r.GetParentTasks()).Returns(_parentTasks);

            // Insert Parent task
            repository.Setup(r => r.AddParentTask(It.IsAny<ParentTaskEntity>()))
                .Callback((ParentTaskEntity p) => _parentTasks.Add(p));

            // Get All tasks by Project Id
            repository.Setup(r => r.GetAllTasks(It.IsAny<int>()))
                .Returns((int i) => _tasks.Where(t => t.ProjectId == i).ToList());

            // Get task by Id
            repository.Setup(r => r.GetTaskById(It.IsAny<int>()))
                .Returns((int i) => _tasks.Where(t => t.TaskId == i).SingleOrDefault());

            // Insert task
            repository.Setup(r => r.AddTask(It.IsAny<TaskEntity>()))
                .Callback((TaskEntity t) => _tasks.Add(t));

            // Update Project
            repository.Setup(r => r.UpdateTask(It.IsAny<TaskEntity>())).Callback(
                (TaskEntity target) =>
                {
                    var original = _tasks.Where(
                        q => q.TaskId == target.TaskId).Single();

                    original.TaskName = target.TaskName;
                    original.ParentId = target.ParentId;
                    original.Priority = target.Priority;
                    original.StartDate = target.StartDate;
                    original.EndDate = target.EndDate;
                    original.ProjectId = target.ProjectId;
                    original.UserId = target.UserId;
                });

            // End Task
            repository.Setup(r => r.EndTask(It.IsAny<int>())).Callback(
                (int taskId) =>
                {
                    var original = _tasks.Where(
                        q => q.TaskId == taskId).Single();

                    original.TaskStatus = "C";
                });

            _mockRepository = repository.Object;
        }

        [Test]
        public void Get_Parent_Tasks()
        {
            List<ParentTaskEntity> parentTasks = _mockRepository.GetParentTasks();

            Assert.IsTrue(parentTasks.Count() == 3);
            Assert.IsTrue(parentTasks.ElementAt(0).TaskName == "Parent1");
            Assert.IsTrue(parentTasks.ElementAt(1).TaskId == 2);
        }

        [Test]
        public void Add_Parent_Task()
        {
            var taskId = _parentTasks.Count() + 1;
            var task = new ParentTaskEntity
            {
                TaskId = taskId,
                TaskName = "Parent4"
            };

            _mockRepository.AddParentTask(task);
            Assert.IsTrue(_parentTasks.Count() == 4);

            ParentTaskEntity testTask = GetParentById(taskId);
            Assert.IsNotNull(testTask);
            Assert.AreSame(testTask.GetType(), typeof(ParentTaskEntity));
            Assert.AreEqual(task.TaskName, testTask.TaskName);
        }

        [Test]
        public void Get_All_tasks_By_Project_Id()
        {
            List<TaskEntity> tasks = _mockRepository.GetAllTasks(1);

            Assert.IsTrue(tasks.Count() == 4);
            Assert.IsTrue(tasks.ElementAt(0).TaskName == "Task1");            
            Assert.IsTrue(tasks.ElementAt(1).Priority == 12);
            Assert.IsTrue(tasks.ElementAt(2).EndDate == "11/15/2018");
            Assert.IsTrue(tasks.ElementAt(3).ProjectId == 1);
            Assert.IsTrue(tasks.ElementAt(0).ParentId == 1);
            Assert.IsTrue(tasks.ElementAt(1).StartDate == "10/16/2018");
        }

        [Test]
        public void Get_Task_By_Id()
        {
            var taskId = 4;

            TaskEntity task = _mockRepository.GetTaskById(taskId);

            Assert.IsNotNull(task);
            Assert.IsTrue(task.TaskName == "Task4");
            Assert.IsTrue(task.UserId == 1234570);
            Assert.IsTrue(task.ProjectName == "Project1");
            Assert.IsTrue(task.EndDate == "11/30/2018");
        }

        [Test]
        public void Add_Task()
        {
            var taskId = _tasks.Count() + 1;
            var task = new TaskEntity
            {
                TaskId = taskId,
                TaskName = "Task6",
                ParentId = 1,
                Priority = 15,
                StartDate = "10/02/2018",
                EndDate = "10/08/2018",
                ProjectId = 1,
                UserId = 1234567,
                TaskStatus = "A"
            };

            _mockRepository.AddTask(task);
            Assert.IsTrue(_tasks.Count() == 6);
            TaskEntity testTask = _mockRepository.GetTaskById(taskId);
            Assert.IsNotNull(testTask);
            Assert.AreSame(testTask.GetType(), typeof(TaskEntity));
            Assert.AreEqual(task.ProjectName, testTask.ProjectName);
            Assert.AreEqual(task.StartDate, testTask.StartDate);
            Assert.AreEqual(task.Priority, testTask.Priority);
            Assert.AreEqual(task.UserId, testTask.UserId);
        }

        [Test]
        public void Update_Task()
        {
            var taskId = 3;
            var task = new TaskEntity
            {
                TaskId = taskId,
                TaskName = "Task Update",
                ParentId = 1,
                Priority = 10,
                StartDate = "11/01/2018",
                EndDate = "11/15/2018",
                ProjectId = 1,
                UserId = 1234571,
            };

            _mockRepository.UpdateTask(task);

            var updatedProject = _mockRepository.GetTaskById(taskId);
            Assert.IsTrue(updatedProject.TaskName == "Task Update");
            Assert.IsTrue(updatedProject.Priority == 10);
            Assert.IsTrue(updatedProject.UserId == 1234571);
        }

        [Test]
        public void End_Task()
        {
            var taskId = 5;

            _mockRepository.EndTask(taskId);

            var completedTask = _mockRepository.GetTaskById(taskId);
            Assert.IsTrue(completedTask.TaskStatus == "C");
        }

        [TearDown]
        public void CleanUp()
        {
            _tasks.Clear();
        }

        private ParentTaskEntity GetParentById(int taskId)
        {
            return _parentTasks.Where(x => x.TaskId == taskId).Select(y => y).SingleOrDefault();
        }
    }
}
