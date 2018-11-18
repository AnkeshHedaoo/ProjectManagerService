using ProjectManager.DL.EntityDataModel;
using ProjectManagerEntity;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;

namespace ProjectManager.DL
{
    public class ProjectDataLayer : IProjectDataLayer
    {
        ProjectManagerSQLConn _db;
        public ProjectDataLayer()
        {
            _db = new ProjectManagerSQLConn();
        }
        public void AddProject(ProjectEntity project)
        {
            var newProj = new T_PROJECT();

            newProj.PROJ_NM = project.ProjectName;
            newProj.PROJ_PRIORITY = project.Priority;
            newProj.PROJ_MGR_ID = project.ProjectManagerId;
            newProj.PROJ_STRT_DT = Utility.GetFormattedDate(project.StartDate);
            newProj.PROJ_END_DT = Utility.GetFormattedDate(project.EndDate);

            _db.T_PROJECT.Add(newProj);
            _db.SaveChanges();
        }

        public List<ProjectEntity> GetAllProjects()
        {
            var projects = (from proj in _db.T_PROJECT
                            join usr in _db.T_USER on proj.PROJ_MGR_ID equals usr.EMP_ID
                            select new
                            {
                                ProjectId = proj.PROJ_ID,
                                ProjectName = proj.PROJ_NM,
                                StartDate = proj.PROJ_STRT_DT,
                                EndDate = proj.PROJ_END_DT,
                                Priority = proj.PROJ_PRIORITY,
                                ProjectManagerId = proj.PROJ_MGR_ID,
                                ProjectManagerFullName = usr.EMP_FRST_NM + " " + usr.EMP_LST_NM,
                                TasksCount = (from t in _db.T_TASK where t.PROJ_ID == proj.PROJ_ID select t).Count(),
                                Completed = (from tsk in _db.T_TASK where tsk.PROJ_ID == proj.PROJ_ID && tsk.STATUS == "C" select tsk).Count()
                            })
                            .ToList()
                            .Select(x => new ProjectEntity
                            {
                                ProjectId = x.ProjectId,
                                ProjectName = x.ProjectName,
                                StartDate = x.StartDate.HasValue ? x.StartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty,
                                EndDate = x.EndDate.HasValue ? x.EndDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty,
                                Priority = x.Priority,
                                ProjectManagerId = x.ProjectManagerId,
                                ProjectManagerFullName = x.ProjectManagerFullName,
                                TasksCount = x.TasksCount,
                                Completed = x.Completed
                            }).ToList();

            return projects;
        }

        public void SuspendProject(int projectId)
        {
            var projToDelete = (from p in _db.T_PROJECT where p.PROJ_ID == projectId select p).FirstOrDefault();
            if (projToDelete != null)
            {
                // First delete the tasks associated with the project
                var tasksToDelete = (from t in _db.T_TASK where t.PROJ_ID == projectId select t).ToList();
                if (tasksToDelete.Count > 0) { _db.T_TASK.RemoveRange(tasksToDelete); }

                // Delete the project
                _db.T_PROJECT.Remove(projToDelete);
                _db.SaveChanges();
            }
        }

        public void UpdateProject(ProjectEntity project)
        {
            var projFromDb = (from proj in _db.T_PROJECT
                              where proj.PROJ_ID == project.ProjectId
                              select proj).FirstOrDefault();

            projFromDb.PROJ_NM = project.ProjectName;
            projFromDb.PROJ_PRIORITY = project.Priority;
            projFromDb.PROJ_MGR_ID = project.ProjectManagerId;
            projFromDb.PROJ_STRT_DT = Utility.GetFormattedDate(project.StartDate);
            projFromDb.PROJ_END_DT = Utility.GetFormattedDate(project.EndDate);

            _db.SaveChanges();
        }
    }
}
