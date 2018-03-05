using System.Collections.Generic;

namespace Domain
{
    public class ApplicationValidationResult
    {
        private ApplicationValidationResult(bool ok, IEnumerable<string> errors)
        {
            Ok = ok;
            Errors = errors;
        }

        public static ApplicationValidationResult OkResult()
        {
            return new ApplicationValidationResult(true, new List<string>());
        }

        public static ApplicationValidationResult ErrorResult(IList<string> errors)
        {
            return new ApplicationValidationResult(false, errors);
        }

        public bool Ok { get; }
        public IEnumerable<string> Errors { get; }
    }
}