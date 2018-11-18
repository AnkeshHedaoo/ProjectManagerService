using NBench;
using System.Collections.Generic;
using ProjectManagerEntity;
using ProjectManager.BL;
using ProjectManager.DL;
using System;

namespace ProjectManager.PerformanceTests
{
    public class ProjectManagerPerfTests
    {
        private IUserBL _userService;
        private IProjectBL _projectService;
        private ITaskBL _taskService;

        private TaskEntity _newTask;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            IUserDataLayer _userRepo = new UserDataLayer();
            IProjectDataLayer _projectRepo = new ProjectDataLayer();
            ITaskDataLayer _taskRepo = new TaskDataLayer();

            _userService = new UserBL(_userRepo);
            _projectService = new ProjectBL(_projectRepo);
            _taskService = new TaskBL(_taskRepo);

            _newTask = new TaskEntity { TaskName = "Perf Test Task", ParentId = 1, Priority = 10, StartDate = "11/01/2018", EndDate = "11/11/2018", ProjectId = 2, UserId = 1234567 };
        }

        [PerfBenchmark(NumberOfIterations = 500, RunMode = RunMode.Iterations, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 100)]
        public void Get_Tasks_500_Iterations()
        {
            _taskService.GetAllTasks(3);
        }

        [PerfBenchmark(RunMode = RunMode.Throughput, RunTimeMilliseconds = 10000, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 3000)]
        public void Get_Tasks_10_Minutes()
        {
            _taskService.GetAllTasks(3);
        }

        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Iterations, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 50)]
        public void Add_Task_Elapsed_Time()
        {
            _taskService.AddTask(_newTask);
        }

        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Iterations, TestMode = TestMode.Test, SkipWarmups = true)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.GreaterThan, ByteConstants.SixtyFourKb)]
        public void Add_Task_Memory_Consumed()
        {
            _taskService.AddTask(_newTask);
        }
    }
}
