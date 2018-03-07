using System;
using System.Threading.Tasks;
using Domain.Users;
using GenericWebservice.Domain;
using GenericWebServiceBuilder.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    public class UserCommandHandler : IUserCommandHandler
    {
        private readonly IEventStore _eventStore;
        private readonly IAggregateStore _aggregateStore;

        public UserCommandHandler(IEventStore eventStore, IAggregateStore aggregateStore)
        {
            _eventStore = eventStore;
            _aggregateStore = aggregateStore;
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

        public async Task<IActionResult> UpdateUserName(UpdateUserNameCommand updateUserNameCommand)
        {
            var user = await _aggregateStore.GetAggregate<User>(updateUserNameCommand.Id);
            if (user is User parsedUser)
            {
                var validationResult = parsedUser.UpdateName(updateUserNameCommand.Name);
                if (validationResult.Ok)
                {
                    _eventStore.AppendAll(validationResult.DomainEvents);
                    return new OkResult();
                }

                return new BadRequestObjectResult(validationResult.DomainErrors);
            }

            return new NotFoundResult();

        }

        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _aggregateStore.GetAggregate<User>(id);
            if (user != null) return new JsonResult(user);

            return new NotFoundResult();
        }
    }
}