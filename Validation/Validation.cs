using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation
{
    public sealed class Validation
    {
        private List<Exception> exceptions;
        private Boolean exceptionsObserved;
        private System.Diagnostics.StackTrace callerTrace;

        public IEnumerable<Exception> Exceptions
        {
            get
            {
                return exceptions;
            }
        }

        public Validation AddException(Exception ex)
        {
            lock(this.exceptions)
            {
                this.exceptions.Add(ex);
            }
            return this;
        }

        public Validation()
        {
            this.exceptions = new List<Exception>(1);
            this.exceptionsObserved = false;
            callerTrace = new System.Diagnostics.StackTrace();
        }

        public void ObserveExceptions()
        {
            exceptionsObserved = true;

            if (this.Exceptions.Take(2).Count() == 1)
                throw new ValidationException(this.Exceptions.First());
            else
                throw new ValidationException("Multiple validation issues occurred", this.Exceptions);
        }

        ~Validation()
        {
            if (!exceptionsObserved)
            {
                if (this.Exceptions.Take(2).Count() == 1)
                    throw new ValidationException(this.Exceptions.First());
                else
                    throw new ValidationException("Multiple validation issues occurred and were unobserved", this.Exceptions);
            }
        }
    }
}
