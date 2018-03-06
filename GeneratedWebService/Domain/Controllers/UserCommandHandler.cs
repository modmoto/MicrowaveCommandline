using System;
using Domain.Users;
using GenericWebservice.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    public class UserCommandHandler : IUserCommandHandler
    {
        private readonly IEventStore _eventStore;

        public UserCommandHandler(IEventStore eventStore)
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

        public IActionResult UpdateUserName(UpdateUserNameCommand updateUserNameCommand)
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
}