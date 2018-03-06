using System;

namespace Domain.Users
{
    public partial class User
    {
        public static CreateUserEvent Create(string name, int age)
        {
            var newGuid = Guid.NewGuid();
            return new CreateUserEvent(new User(newGuid, name, age));
        }

        public ValidationResult UpdateAge(int Age)
        {
            throw new NotImplementedException();
        }

        public ValidationResult UpdateName(string Name)
        {
            throw new NotImplementedException();
        }
    }
}