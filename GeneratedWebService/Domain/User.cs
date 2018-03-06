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
        public UserUpdateAgeEvent UpdateAge(int Age)
        {
            throw new System.NotImplementedException();
        }

        public UserUpdateNameEvent UpdateName(string Name)
        {
            throw new System.NotImplementedException();
        }
    }
}