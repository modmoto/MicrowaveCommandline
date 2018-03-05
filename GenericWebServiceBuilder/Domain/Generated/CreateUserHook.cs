using System.Collections.Generic;
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

    public class ApplicationResult
    {
        private ApplicationResult(bool ok, IEnumerable<string> errors)
        {
            Ok = ok;
            Errors = errors;
        }

        public static ApplicationResult OkResult()
        {
            return new ApplicationResult(true, new List<string>());
        }

        public static ApplicationResult ErrorResult(IList<string> errors)
        {
            return new ApplicationResult(false, errors);
        }

        public bool Ok { get; }
        public IEnumerable<string> Errors { get; }
    }
}