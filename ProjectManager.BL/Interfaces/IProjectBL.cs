using ProjectManagerEntity;
using System.Collections.Generic;

namespace ProjectManager.BL
{
    public interface IProjectBL
    {
        List<ProjectEntity> GetAllProjects();
        void AddProject(ProjectEntity project);
        void UpdateProject(ProjectEntity project);
        void SuspendProject(int projectId);
    }
}
