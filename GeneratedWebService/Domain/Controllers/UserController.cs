using System;
using System.Collections.Generic;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;

        protected UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return _repository.GetUser(id);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserCommand createUserCommand)
        {
            return _repository.CreateUser(createUserCommand);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserName(Guid id, [FromBody] string name)
        {
            var updateUserNameCommand = new UpdateUserNameCommand(id, name);
            return _repository.UpdateUserName(updateUserNameCommand);
        }
    }
}