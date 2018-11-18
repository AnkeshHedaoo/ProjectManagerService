using ProjectManagerEntity;
using System.Collections.Generic;

namespace ProjectManager.DL
{
    public interface ITaskDataLayer
    {
        List<ParentTaskEntity> GetParentTasks();
        void AddParentTask(ParentTaskEntity task);

        List<TaskEntity> GetAllTasks(int projectId);
        TaskEntity GetTaskById(int taskId);
        void AddTask(TaskEntity task);
        void UpdateTask(TaskEntity task);
        void EndTask(int taskId);
    }
}
