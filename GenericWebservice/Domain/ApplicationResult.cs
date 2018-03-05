using System.Collections.Generic;

namespace Domain
{
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