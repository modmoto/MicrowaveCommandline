using System;
using Domain.Users;

namespace Application.Users.Hooks
{
    public partial class CreateUserEventHook : IDomainHook
    {
        public Type EventType { get; }

        public CreateUserEventHook()
        {
            EventType = typeof(UserCreateEvent);
        }

    }
}