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
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private IUserBL _userService;
        public UserController(IUserBL service)
        {
            _userService = service;
        }

        [HttpGet]
        [Route("getallusers")]
        public IHttpActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create([FromBody]UserEntity userModel)
        {
            _userService.AddUser(userModel);
            return Ok();
        }

        [HttpPut]
        [Route("update")]
        public IHttpActionResult Update([FromBody]UserEntity userModel)
        {
            _userService.UpdateUser(userModel);
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult Delete(int userId)
        {
            _userService.DeleteUser(userId);
            return Ok();
        }
    }
}