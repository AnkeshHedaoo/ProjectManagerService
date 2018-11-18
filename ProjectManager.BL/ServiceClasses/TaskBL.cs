using ProjectManagerEntity;
using System.Collections.Generic;
using System;
using ProjectManager.DL;

namespace ProjectManager.BL
{
    public class TaskBL : ITaskBL
    {
        private ITaskDataLayer _repo;

        public TaskBL(ITaskDataLayer repo)
        {
            _repo = repo;
        }
        public List<ParentTaskEntity> GetParentTasks()
        {
            return _repo.GetParentTasks();
        }

        public void AddParentTask(ParentTaskEntity task)
        {
            _repo.AddParentTask(task);
        }

        public List<TaskEntity> GetAllTasks(int projectId)
        {
            return _repo.GetAllTasks(projectId);
        }

        public TaskEntity GetTaskById(int taskId)
        {
            return _repo.GetTaskById(taskId);
        }

        public void AddTask(TaskEntity task)
        {
            _repo.AddTask(task);
        }

        public void EndTask(int taskId)
        {
            _repo.EndTask(taskId);
        }

        public void UpdateTask(TaskEntity task)
        {
            _repo.UpdateTask(task);
        }
    }
}
