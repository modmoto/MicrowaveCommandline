using System;
using System.Collections.Generic;
using Domain;
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
        public User Get(int id)
        {
            return null;
        }

        [HttpPost]
        public User CreateUser([FromBody]CreateUserCommand createUserCommand)
        {
            var res = _repo.CreateUser(createUserCommand);
            return null;
        }

        [HttpPut("{id}")]
        public User UpdateUserName(Guid id, [FromBody]string name)
        {
            return null;
        }
    }

    public class UserRepo
    {
        public DomainValidationResult CreateUser(CreateUserCommand createUserCommand)
        {
            using (var store = new AggregateStoreContext())
            {
                var createUserEvent = User.Create(createUserCommand.Name, createUserCommand.Age);
                if (createUserEvent != null)
                {
                    store.Users.Add(createUserEvent.User);
                    return DomainValidationResult.OkResult(null);
                }
                else
                {
                    return DomainValidationResult.ErrorResult(null);
                }
            }
        }
    }

    public class CreateUserCommand
    {
        public string Name { get; }
        public int Age { get; }

        public CreateUserCommand(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
