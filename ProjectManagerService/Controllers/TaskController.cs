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
    [RoutePrefix("api/task")]
    public class TaskController : ApiController
    {
        private ITaskBL _taskService;
        public TaskController(ITaskBL service)
        {
            _taskService = service;
        }

        #region Parent Tasks

        [HttpGet]
        [Route("getparenttasks")]
        public IHttpActionResult GetParentTasks()
        {
            var result = _taskService.GetParentTasks();
            return Ok(result);
        }

        [HttpPost]
        [Route("createparenttask")]
        public IHttpActionResult CreateParentTask([FromBody]ParentTaskEntity taskModel)
        {
            _taskService.AddParentTask(taskModel);
            return Ok();
        }

        #endregion

        #region Tasks

        [HttpGet]
        [Route("getalltasks")]
        public IHttpActionResult GetAllTasks(int projectId)
        {
            var result = _taskService.GetAllTasks(projectId);
            return Ok(result);
        }

        [HttpGet]
        [Route("gettaskbyid")]
        public IHttpActionResult GetTaskById(int taskId)
        {
            var result = _taskService.GetTaskById(taskId);
            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create([FromBody]TaskEntity taskModel)
        {
            _taskService.AddTask(taskModel);
            return Ok();
        }


        [HttpPut]
        [Route("update")]
        public IHttpActionResult Update([FromBody]TaskEntity taskModel)
        {
            _taskService.UpdateTask(taskModel);
            return Ok();
        }

        [HttpPut]
        [Route("endtask")]
        public IHttpActionResult EndTask(int taskId)
        {
            _taskService.EndTask(taskId);
            return Ok();
        }

        #endregion
    }
}