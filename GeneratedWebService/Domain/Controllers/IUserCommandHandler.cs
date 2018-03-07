using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    public interface IUserCommandHandler
    {
        Task<IActionResult> CreateUser(CreateUserCommand createUserCommand);
        Task<IActionResult> UpdateUserName(UpdateUserNameCommand updateUserNameCommand);
        Task<IActionResult> GetUser(Guid id);
    }
}