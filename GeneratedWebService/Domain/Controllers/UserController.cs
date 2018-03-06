using System;
using System.Collections.Generic;
using Domain.Users;
using GenericWebservice.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepo _repo;

        protected UserController(UserRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return _repo.GetUser(id);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserCommand createUserCommand)
        {
            return _repo.CreateUser(createUserCommand);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserName(Guid id, [FromBody] string name)
        {
            var updateUserNameCommand = new UpdateUserNameCommand(id, name);
            return _repo.UpdateUser(updateUserNameCommand);
        }
    }

    public class UserRepo
    {
        private readonly IEventStore _eventStore;

        public UserRepo(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public IActionResult CreateUser(CreateUserCommand createUserCommand)
        {
            var createUserResult = User.Create(createUserCommand.Name, createUserCommand.Age);
            if (createUserResult.Ok)
            {
                _eventStore.AppendAll(createUserResult.DomainEvents);
                return new CreatedResult("uri", null);
            }

            return new BadRequestObjectResult(createUserResult.DomainErrors);
        }

        public IActionResult UpdateUser(UpdateUserNameCommand updateUserNameCommand)
        {
            var user = _eventStore.Load<User>(updateUserNameCommand.Id);
            if (user != null)
            {
                var validationResult = user.UpdateName(updateUserNameCommand.Name);
                if (validationResult.Ok)
                {
                    _eventStore.AppendAll(validationResult.DomainEvents);
                    return new OkResult();
                }

                return new BadRequestObjectResult(validationResult.DomainErrors);
            }

            return new NotFoundResult();
        }

        public IActionResult GetUser(Guid id)
        {
            var user = _eventStore.Load<User>(id);
            if (user != null) return new JsonResult(user);

            return new NotFoundResult();
        }
    }

    public class UpdateUserNameCommand
    {
        public UpdateUserNameCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }

    public class CreateUserCommand
    {
        public CreateUserCommand(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }
}