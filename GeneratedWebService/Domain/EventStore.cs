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
        Task<HookResult> AppendAll(List<DomainEventBase> domainEvents);
    }

    public class EventStore : IEventStore
    {
        public EventStore()
        {
            DomainHooks = new List<IDomainHook> {new CreateUserEventHook()};
        }

        public IEnumerable<IDomainHook> DomainHooks { get; }

        public async Task<HookResult> AppendAll(List<DomainEventBase> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var domainHooks = DomainHooks.Where(hook => hook.Event.GetType() == domainEvent.GetType());
                foreach (var domainHook in domainHooks)
                {
                    var validationResult = domainHook.Execute(domainEvent);
                    if (!validationResult.Ok)
                        return validationResult;
                }
            }

            using (var context = new EventStoreContext())
            {
                context.EventHistory.AddRange(domainEvents);
                await context.SaveChangesAsync();
                return HookResult.OkResult();
            }
        }
    }

    public interface IDomainHook
    {
        DomainEventBase Event { get; }
        HookResult Execute(DomainEventBase domainEvent);
    }

    public partial class CreateUserEventHook : IDomainHook
    {
        public DomainEventBase Event { get; }
    }

    public partial class CreateUserEventHook
    {
        public HookResult Execute(DomainEventBase domainEvent)
        {
            if (domainEvent is CreateUserEvent parsedEvent)
            {
                var newUserAge = parsedEvent.User.Age + 10;
                var domainEventBases = new List<DomainEventBase>();
                domainEventBases.Add(new UserUpdateAgeEvent(newUserAge, Guid.NewGuid()));
                return HookResult.OkResult();
            }
            return HookResult.ErrorResult(new List<string> {"Irgend ein fehler"});
        }
    }

    public class HookResult
    {
        private HookResult(List<string> errors)
        {
            Errors = errors;
        }

        public static HookResult OkResult()
        {
            return new HookResult(new List<string>());
        }

        public static HookResult ErrorResult(List<string> errors)
        {
            return new HookResult(errors);
        }

        public List<string> Errors { get; }

        public bool Ok => Errors.Count > 0;
    }
}