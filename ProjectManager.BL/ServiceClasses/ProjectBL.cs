using ProjectManagerEntity;
using System.Collections.Generic;
using System;
using ProjectManager.DL;

namespace ProjectManager.BL
{
    public class ProjectBL : IProjectBL
    {
        private IProjectDataLayer _repo;

        public ProjectBL(IProjectDataLayer repo)
        {
            _repo = repo;
        }
        public List<ProjectEntity> GetAllProjects()
        {
            return _repo.GetAllProjects();
        }

        public void SuspendProject(int projectId)
        {
            _repo.SuspendProject(projectId);
        }

        public void AddProject(ProjectEntity project)
        {
            _repo.AddProject(project);
        }

        public void UpdateProject(ProjectEntity project)
        {
            _repo.UpdateProject(project);
        }
    }
}
