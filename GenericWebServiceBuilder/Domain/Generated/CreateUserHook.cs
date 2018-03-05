using System.Collections.Immutable;

namespace Domain
{
    public partial class CreateUserHook : ICreateUserHook
    {
    }

    public interface ICreateUserHook
    {
        ApplicationResult SendMailSynchronous(UserCreateEvent createEvent);
        ApplicationResult SendMailAsync(UserCreateEvent createEvent);
    }
}