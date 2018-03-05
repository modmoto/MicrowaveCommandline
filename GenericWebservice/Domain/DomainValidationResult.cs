using System;
using System.Collections.Generic;

namespace Domain
{
    public class DomainValidationResult
    {
        private DomainValidationResult(bool ok, IEnumerable<IDomainEvent> domainEvents, IEnumerable<DomainError> domainErrors)
        {
            Ok = ok;
            DomainEvents = domainEvents;
            DomainErrors = domainErrors;
        }

        public bool Ok { get; }
        public IEnumerable<IDomainEvent> DomainEvents { get; }
        public IEnumerable<DomainError> DomainErrors { get; }

        public static DomainValidationResult OkResult(IEnumerable<IDomainEvent> domainEvents)
        {
            return new DomainValidationResult(true, domainEvents, new List<DomainError>());
        }

        public static DomainValidationResult ErrorResult(IEnumerable<DomainError> domainErrors)
        {
            return new DomainValidationResult(false, new List<IDomainEvent>(), domainErrors);
        }
    }

    public class DomainError
    {
        public DomainError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public String ErrorMessage { get; }
    }

    public interface IDomainEvent
    {
    }
}