using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Users;
using GenericWebServiceBuilder.Domain;
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
        public IActionResult CreateUser(CreateUserCommand createUserCommand)
        {
            using (var store = new AggregateStoreContext())
            {
                var createUserResult = User.Create(createUserCommand.Name, createUserCommand.Age);
                if (createUserResult.Ok)
                {
                    var domainEventBases = createUserResult.DomainEvents.Select(ev => (CreateUserEvent) ev);
                    var usersCreated = domainEventBases.Select(ev => ev.User);
                    store.Users.AddRange(usersCreated);
                    return new CreatedResult("uri", usersCreated);
                }

                return new BadRequestResult();
            }
        }

        public IActionResult UpdateUser(UpdateUserNameCommand updateUserNameCommand)
        {
            using (var store = new AggregateStoreContext())
            {
                var users = store.Users.Where(user => user.Id == updateUserNameCommand.Id);
                if (users.Count() == 1)
                {
                    var user = users.First();
                    var validationResult = user.UpdateName(updateUserNameCommand.Name);
                    if (validationResult.Ok)
                    {
                        return new OkResult();
                    }
                    return new BadRequestResult();
                }

                return new NotFoundResult();
            }
        }

        public IActionResult GetUser(Guid id)
        {
            using (var store = new AggregateStoreContext())
            {
                var users = store.Users.Where(user => user.Id == id);
                if (users.Count() == 1)
                {
                    var user = users.First();
                    return new JsonResult(user);
                }

                return new NotFoundResult();
            }
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