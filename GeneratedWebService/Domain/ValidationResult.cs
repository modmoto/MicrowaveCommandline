using System.Collections.Generic;

namespace Domain
{
    public class ValidationResult
    {
        private ValidationResult(bool ok, IList<IDomainEvent> domainEvents, IList<string> domainErrors)
        {
            Ok = ok;
            DomainEvents = domainEvents;
            DomainErrors = domainErrors;
        }

        public bool Ok { get; }
        public IList<IDomainEvent> DomainEvents { get; }
        public IList<string> DomainErrors { get; }

        public static ValidationResult OkResult(IList<IDomainEvent> domainEvents)
        {
            return new ValidationResult(true, domainEvents, new List<string>());
        }

        public static ValidationResult ErrorResult(IList<string> domainErrors)
        {
            return new ValidationResult(false, new List<IDomainEvent>(), domainErrors);
        }
    }

    public interface IDomainEvent
    {
    }
}