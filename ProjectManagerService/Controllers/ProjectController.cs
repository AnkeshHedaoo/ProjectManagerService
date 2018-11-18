using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjectManager.BL;
using ProjectManagerEntity;

namespace ProjectManagerService.Controllers
{
    [RoutePrefix("api/project")]
    public class ProjectController : ApiController
    {
        private IProjectBL _projectService;
        public ProjectController(IProjectBL service)
        {
            _projectService = service;
        }

        [HttpGet]
        [Route("getallprojects")]
        public IHttpActionResult GetAllProjects()
        {
            var result = _projectService.GetAllProjects();
            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create([FromBody]ProjectEntity projectModel)
        {
            _projectService.AddProject(projectModel);
            return Ok();
        }

        [HttpPut]
        [Route("update")]
        public IHttpActionResult Update([FromBody]ProjectEntity projectModel)
        {
            _projectService.UpdateProject(projectModel);
            return Ok();
        }

        [HttpDelete]
        [Route("suspend")]
        public IHttpActionResult Suspend(int projectId)
        {
            _projectService.SuspendProject(projectId);
            return Ok();
        }
    }
}