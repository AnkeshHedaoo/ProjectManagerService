using ProjectManagerEntity;
using System.Collections.Generic;

namespace ProjectManager.DL
{
    public interface IProjectDataLayer
    {
        List<ProjectEntity> GetAllProjects();
        void AddProject(ProjectEntity project);
        void UpdateProject(ProjectEntity project);
        void SuspendProject(int projectId);
    }
}
