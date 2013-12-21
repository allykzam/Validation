using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation
{
    /// <summary>
    /// These methods are used to bootstrap the methods in the Extensions class, as they need a way to check the arguments passed to themselves
    /// </summary>
    internal static class InternalValidation
    {
        internal static void IsNotNull<T>(T value, String name) where T : class
        {
            if (name == null)
                throw new ArgumentNullException("name", String.Format(Extensions.ArgumentNull, "name"));

            if (name == "")
                throw new ArgumentException(String.Format(Extensions.ArgumentEmpty, "name"));

            if (value == null)
                throw new ArgumentNullException(name, String.Format(Extensions.ArgumentNull, name));
        }

        internal static void IsNotNull<T>(T? value, String name) where T : struct
        {
            IsNotNull(name, "name");

            if (name == "")
                throw new ArgumentException(String.Format(Extensions.ArgumentEmpty, "name"));

            if (!value.HasValue)
                throw new ArgumentNullException(name, String.Format(Extensions.ArgumentNull, name));
        }

        internal static void IsNotNullOrEmpty(String value, String name)
        {
            IsNotNull(name, "name");
            IsNotNull(value, "value");

            if (value == "")
                throw new ArgumentException(String.Format(Extensions.ArgumentEmpty, "name"));
        }
    }
}
