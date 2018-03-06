using System;
using System.Collections.Generic;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserCommandHandler _commandHandler;

        protected UserController(IUserCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return _commandHandler.GetUser(id);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserCommand createUserCommand)
        {
            return _commandHandler.CreateUser(createUserCommand);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserName(Guid id, [FromBody] string name)
        {
            var updateUserNameCommand = new UpdateUserNameCommand(id, name);
            return _commandHandler.UpdateUserName(updateUserNameCommand);
        }
    }
}