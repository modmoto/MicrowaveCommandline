using System;
using System.Threading.Tasks;
using Domain.Users;
using GenericWebservice.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    public class UserCommandHandler : IUserCommandHandler
    {
        private readonly IEventStore _eventStore;
        private readonly IUserRepository _userRepository;

        public UserCommandHandler(IEventStore eventStore, IUserRepository userRepository)
        {
            _eventStore = eventStore;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> CreateUser(CreateUserCommand createUserCommand)
        {
            var createUserResult = User.Create(createUserCommand.Name, createUserCommand.Age);
            if (createUserResult.Ok)
            {
                _eventStore.Append(createUserResult.DomainEvents);
                await _userRepository.CreateUser(createUserResult.CreatedEntity);

                return new CreatedResult("uri", createUserResult.CreatedEntity);
            }

            return new BadRequestObjectResult(createUserResult.DomainErrors);
        }

        public async Task<IActionResult> UpdateUserName(UpdateUserNameCommand updateUserNameCommand)
        {
            var user = await _userRepository.GetUser(updateUserNameCommand.Id);
            if (user is User parsedUser)
            {
                var validationResult = parsedUser.UpdateName(updateUserNameCommand.Name);
                if (validationResult.Ok)
                {
                    await _userRepository.UpdateUser(parsedUser);
                    _eventStore.Append(validationResult.DomainEvents);
                    return new OkResult();
                }

                return new BadRequestObjectResult(validationResult.DomainErrors);
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userRepository.GetUser(id);
            if (user != null) return new JsonResult(user);

            return new NotFoundResult();
        }
    }
}