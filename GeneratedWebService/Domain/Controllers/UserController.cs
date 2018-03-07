using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Get(Guid id)
        {
            return await _commandHandler.GetUser(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand createUserCommand)
        {
            return await _commandHandler.CreateUser(createUserCommand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserName(Guid id, [FromBody] string name)
        {
            var updateUserNameCommand = new UpdateUserNameCommand(id, name);
            return await _commandHandler.UpdateUserName(updateUserNameCommand);
        }
    }
}