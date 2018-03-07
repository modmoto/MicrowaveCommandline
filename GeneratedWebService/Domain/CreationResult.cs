using System;
using System.Collections.Generic;

namespace Domain.Users
{
    public class CreationResult<T>
    {
        private CreationResult(Boolean ok, T createdEntity, IList<string> domainErrors, List<DomainEventBase> domainEvents)
        {
            CreatedEntity = createdEntity;
            DomainErrors = domainErrors;
            DomainEvents = domainEvents;
            Ok = ok;
        }

        public T CreatedEntity { get; }
        public IList<string> DomainErrors { get; }
        public bool Ok { get; }
        public List<DomainEventBase> DomainEvents { get; }

        public static CreationResult<User> OkResult(User user, List<DomainEventBase> events)
        {
            return new CreationResult<User>(true, user, new List<string>(), events);
        }

        public static CreationResult<User> ErrorResult(List<string> errors)
        {
            return new CreationResult<User>(false, null, errors, new List<DomainEventBase>());
        }
    }
}