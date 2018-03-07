using System;
using System.Collections.Generic;

namespace Domain.Users
{
    public partial class User
    {
        public ValidationResult UpdateAge(int Age)
        {
            throw new NotImplementedException();
        }

        public ValidationResult UpdateName(string Name)
        {
            throw new NotImplementedException();
        }

        public static CreationResult<User> Create(string name, int age)
        {
            var newGuid = Guid.NewGuid();
            if (age > 0)
            {
                var user = new User(newGuid, name, age);
                return CreationResult<User>.OkResult(user,
                    new List<DomainEventBase> {new CreateUserEvent(user, newGuid)});
            }

            return CreationResult<User>.ErrorResult(new List<string> {"Age Can not be negative"});
        }
    }
}