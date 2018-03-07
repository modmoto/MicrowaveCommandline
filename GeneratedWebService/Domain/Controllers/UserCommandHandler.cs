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
                var hookResult = await _eventStore.AppendAll(createUserResult.DomainEvents);
                if (hookResult.Ok)
                {
                    await _userRepository.CreateUser(createUserResult.CreatedEntity);
                    return new CreatedResult("uri", createUserResult.CreatedEntity);
                }

                return new BadRequestObjectResult(hookResult.Errors);
            }

            return new BadRequestObjectResult(createUserResult.DomainErrors);
        }

        public async Task<IActionResult> UpdateUserName(UpdateUserNameCommand updateUserNameCommand)
        {
            var user = await _userRepository.GetUser(updateUserNameCommand.Id);
            if (user != null)
            {
                var validationResult = user.UpdateName(updateUserNameCommand.Name);
                if (validationResult.Ok)
                {
                    var hookResult = await _eventStore.AppendAll(validationResult.DomainEvents);
                    if (hookResult.Ok)
                    {
                        await _userRepository.UpdateUser(user);
                        return new OkResult();
                    }

                    return new BadRequestObjectResult(hookResult.Errors);
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