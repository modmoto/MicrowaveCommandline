using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Users;

namespace GenericWebservice.Domain
{
    public interface IEventStore
    {
        Task<ValidationResult> AppendAll(List<DomainEventBase> domainEvents);
    }

    public class EventStore : IEventStore
    {
        public async Task<ValidationResult> AppendAll(List<DomainEventBase> domainEvents)
        {
            var validationResultZusammen = ValidationResult.OkResult(new List<DomainEventBase>());
            foreach (var domainEvent in domainEvents)
            {
                var domainHooks = DomainHooks.Where(hook => hook.Event.GetType() == domainEvent.GetType());
                foreach (var domainHook in domainHooks)
                {
                    var validationResult = domainHook.Execute(domainEvent);
                    if (!validationResult.Ok)
                    {
                        validationResultZusammen.DomainErrors.AddRange(validationResult.DomainErrors);
                    }
                    else
                    {
                        validationResultZusammen.DomainEvents.AddRange(validationResult.DomainEvents);
                    }
                }
            }

            if (validationResultZusammen.Ok)
            {
                var newEventsResult = await AppendAll(validationResultZusammen.DomainEvents);
                return newEventsResult;
            }

            using (var context = new EventStoreContext())
            {
                context.EventHistory.AddRange(domainEvents);
                await context.SaveChangesAsync();
                return validationResultZusammen;
            }
        }

        public IEnumerable<IDomainHook> DomainHooks { get; set; }
    }

    public interface IDomainHook
    {
        DomainEventBase Event { get; }
        ValidationResult Execute(DomainEventBase domainEvent);
    }

    public partial class CreateUserEventHook : IDomainHook
    {
        public DomainEventBase Event { get; }
    }

    public partial class CreateUserEventHook
    {
        public ValidationResult Execute(DomainEventBase domainEvent)
        {
            if (domainEvent is CreateUserEvent parsedEvent)
            {
                var newUserAge = parsedEvent.User.Age + 10;
                var domainEventBases = new List<DomainEventBase>();
                domainEventBases.Add(new UserUpdateAgeEvent(newUserAge, Guid.NewGuid()));
                return ValidationResult.OkResult(domainEventBases);
            }
            return ValidationResult.ErrorResult(new List<string>{ "Irgend ein fehler"});
        }
    }
}