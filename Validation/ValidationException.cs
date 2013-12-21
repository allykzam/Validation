using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Validation
{
    public class ValidationException : AggregateException
    {
        private String stackTrace = GetStackTrace();
        private const String defaultMessage = "Validation exceptions occurred";

        [HideFromStackTrace]
        private static String GetStackTrace()
        {
            var stack = new StackTrace();
            var allFrames = stack.GetFrames();
            var hiddenFrames = allFrames.Select((x, i) => Tuple.Create(x, i)).Where((x) => x.Item1.GetMethod().ShouldHide()).Select((x) => x.Item2);
            var validFrames = allFrames;
            if (hiddenFrames.Take(2).Count() > 0)
                validFrames = allFrames.Skip(hiddenFrames.Last() + 1).ToArray();
            return String.Concat(validFrames.Select((x) => new StackTrace(x).ToString()));
        }

        [HideFromStackTrace]
        public ValidationException() : base(defaultMessage)
        { }

        [HideFromStackTrace]
        public ValidationException(Exception innerException) : base(defaultMessage, innerException)
        { }

        [HideFromStackTrace]
        public ValidationException(String message) : base(message)
        { }

        [HideFromStackTrace]
        public ValidationException(String message, Exception innerException) : base(message, innerException)
        { }

        [HideFromStackTrace]
        public ValidationException(IEnumerable<Exception> innerExceptions) : base(defaultMessage, innerExceptions)
        { }

        [HideFromStackTrace]
        public ValidationException(Exception[] innerExceptions) : base(defaultMessage, innerExceptions)
        { }

        [HideFromStackTrace]
        public ValidationException(String message, IEnumerable<Exception> innerExceptions) : base(message, innerExceptions)
        { }

        [HideFromStackTrace]
        public ValidationException(String message, Exception[] innerExceptions) : base(message, innerExceptions)
        { }

        public new String StackTrace
        {
            get
            {
                return stackTrace;
            }
        }

        public new String Message
        {
            get
            {
                return base.Message + "\r\n\r\n" + base.InnerExceptions.Select((x, i) => String.Format("Message for exception {0} of type {1} is:\r\n{2}", i, x.GetType().ToString(), x.Message));
            }
        }
    }
}
