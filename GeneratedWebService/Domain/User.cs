using System;
using System.Collections.Generic;

namespace Domain.Users
{
    public partial class User
    {
//        public static ValidationResult Create(string name, int age)
//        {
//            var newGuid = Guid.NewGuid();
//            var domainEventBases = new List<DomainEventBase>();
//            domainEventBases.Add(new CreateUserEvent(new User(newGuid, name, age)));
//            return new ValidationResult(true, domainEventBases,  new List<string>());
//        }

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