using System;
using System.Collections.Generic;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
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
        public User CreateUser([FromBody]string name, int age)
        {
            var createUserCommand = new CreateUserCommand(name, age);
            return createUserCommand.Run();
        }

        [HttpPut("{id}")]
        public User UpdateUserName(Guid id, [FromBody]string name)
        {
            return null;
        }
    }

    public class CreateUserCommand
    {
        private readonly string _name;
        private int _age;

        public CreateUserCommand(string name, int age)
        {
            _name = name;
            _age = age;
        }


        public User Run()
        {
            return User.Create(_name, _age).User;
        }
    }
}
