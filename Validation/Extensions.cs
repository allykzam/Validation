using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation
{
    public static class Extensions
    {
        internal const String ArgumentNull = "{0} is null; a value must be provided";
        internal const String ArgumentEmpty = "{0} is empty; a value must be provided";
        internal const String ArgumentOutOfRange = "{0}'s value of \"{1}\" is out of range; must be a value from \"{2}\" to \"{3}\"";
        internal const String ArgumentNotNumeric = "{0} must be a numeric string; actual value is \"{1}\"";
        internal const String ArgumentBooleanWrong = "{0} must be {1}, but was {2}";

        /// <summary>
        /// Checks a value to ensure that it is not null
        /// </summary>
        /// <typeparam name="T">Any object type which is a class, and can therefore be null</typeparam>
        /// <param name="validation">The Validation instance to carry forward</param>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null, passed on in the return when the value is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string</exception>
        public static Validation IsNotNull<T>(this Validation validation, T value, String name) where T : class
        {
            InternalValidation.IsNotNullOrEmpty(name, "name");

            if (value == null)
                return validation.GetInstance().AddException(new ArgumentNullException(name, String.Format(ArgumentNull, name)));

            return validation;
        }

        /// <summary>
        /// If the previous condition was met, checks a value to ensure that it is not null
        /// </summary>
        /// <typeparam name="T">Any object type which is a class, and can therefore be null</typeparam>
        /// <param name="validation">The Validation instance to carry forward, along with the result of the previous condition</param>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null, passed on in the return when the value is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string</exception>
        public static Validation IsNotNull<T>(this Tuple<Validation, Boolean> validation, T value, String name) where T : class
        {
            if (validation.Item2)
                return validation.Item1.IsNotNull(value, name);
            else
                return validation.Item1;
        }

        /// <summary>
        /// Checks a value to ensure that it is not null
        /// </summary>
        /// <typeparam name="T">Any object type which is a struct, and therefore cannot be null by itself</typeparam>
        /// <param name="validation">The Validation instance to carry forward</param>
        /// <param name="value">An instance of T? to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null, passed on in the return when the value is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string</exception>
        public static Validation IsNotNull<T>(this Validation validation, T? value, String name) where T : struct
        {
            InternalValidation.IsNotNullOrEmpty(name, "name");

            if (!value.HasValue)
                return validation.GetInstance().AddException(new ArgumentNullException(name, String.Format(ArgumentNull, name)));

            return validation;
        }

        /// <summary>
        /// If the previous condition was met, checks a value to ensure that it is not null
        /// </summary>
        /// <typeparam name="T">Any object type which is a struct, and therefore cannot be null by itself</typeparam>
        /// <param name="validation">The Validation instance to carry forward, along with the result of the previous condition</param>
        /// <param name="value">An instance of T? to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null, passed on in the return when the value is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string</exception>
        public static Validation IsNotNull<T>(this Tuple<Validation, Boolean> validation, T? value, String name) where T : struct
        {
            if (validation.Item2)
                return validation.Item1.IsNotNull(value, name);
            else
                return validation.Item1;
        }

        /// <summary>
        /// Checks a String value to ensure that it is not null or empty
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward</param>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null, passed on in the return when the value is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is empty, passed on in the return when the value is empty</exception>
        public static Validation IsNotNullOrEmpty(this Validation validation, String value, String name)
        {
            InternalValidation.IsNotNullOrEmpty(name, "name");

            if (value == null)
                return validation.GetInstance().AddException(new ArgumentNullException(name, String.Format(ArgumentNull, name)));

            if (value == "")
                return validation.GetInstance().AddException(new ArgumentException(String.Format(ArgumentEmpty, name)));

            return validation;
        }

        /// <summary>
        /// If the previous condition was met, checks a String value to ensure that it is not null or empty
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward, along with the result of the previous condition</param>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null, passed on in the return when the value is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is empty, passed on in the return when the value is empty</exception>
        public static Validation IsNotNullOrEmpty(this Tuple<Validation, Boolean> validation, String value, String name)
        {
            if (validation.Item2)
                return validation.Item1.IsNotNullOrEmpty(value, name);
            else
                return validation.Item1;
        }

        /// <summary>
        /// Checks a comparable value to ensure that it is within the specified range
        /// </summary>
        /// <typeparam name="T">Any object type which is a class and implements IComparable</typeparam>
        /// <param name="validation">The validation instance to carry forward</param>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the value</param>
        /// <param name="min">The minimum accepted value</param>
        /// <param name="max">The maximum accepted value</param>
        /// <exception cref="ArgumentException">Thrown if min is greater than max</exception>
        /// <exception cref="ValidationException">Thrown if min or max is null, or the name is an empty string</exception>
        /// <exception cref="ArgumentOutOfRange">Passed on in the return when the value is not within the expected range</exception>
        /// <exception cref="ArgumentNullException">Passed on in the return when the value is null</exception>
        public static Validation IsWithinRange<T>(this Validation validation, T value, String name, T min, T max) where T : class, IComparable
        {
            Validate.Begin()
                .IsNotNullOrEmpty(name, "name")
                .IsNotNull(min, "min")
                .IsNotNull(max, "max")
                .Check();

            if (min.CompareTo(max) > 0)
                throw new ArgumentException("min must be equal to or less than max");

            if (value == null)
                return validation.GetInstance().AddException(new ArgumentNullException(name, String.Format(ArgumentNull, name)));

            if (min.CompareTo(value) > 0 || max.CompareTo(value) < 0)
                return validation.GetInstance().AddException(new ArgumentOutOfRangeException(name, String.Format(ArgumentOutOfRange, name, value, min, max)));

            return validation;
        }

        /// <summary>
        /// If the previous condition was met, checks a comparable value to ensure that it is within the specified range
        /// </summary>
        /// <typeparam name="T">Any object type which is a class and implements IComparable</typeparam>
        /// <param name="validation">The validation instance to carry forward, along with the result of the previous condition</param>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the value</param>
        /// <param name="min">The minimum accepted value</param>
        /// <param name="max">The maximum accepted value</param>
        /// <exception cref="ArgumentException">Thrown if min is greater than max</exception>
        /// <exception cref="ValidationException">Thrown if min or max is null, or the name is an empty string</exception>
        /// <exception cref="ArgumentOutOfRange">Passed on in the return when the value is not within the expected range</exception>
        /// <exception cref="ArgumentNullException">Passed on in the return when the value is null</exception>
        public static Validation IsWithinRange<T>(this Tuple<Validation, Boolean> validation, T value, String name, T min, T max) where T : class, IComparable
        {
            if (validation.Item2)
                return validation.Item1.IsWithinRange(value, name, min, max);
            else
                return validation.Item1;
        }

        /// <summary>
        /// Checks a comparable value to ensure that it is within the specified range
        /// </summary>
        /// <typeparam name="T">Any object type which is a struct, and therefore cannot be null by itself, and which implements IComparable</typeparam>
        /// <param name="validation">The validation instance to carry forward</param>
        /// <param name="value">An instance of T? to check</param>
        /// <param name="name">The name of the value</param>
        /// <param name="min">An instance of T? which acts as the minimum accepted value</param>
        /// <param name="max">An instance of T? which acts as the maximum accepted value</param>
        /// <exception cref="ArgumentException">Thrown if min is greater than max</exception>
        /// <exception cref="ValidationException">Thrown if min or max is null, or the name is an empty string</exception>
        /// <exception cref="ArgumentOutOfRange">Passed on in the return when the value is not within the expected range</exception>
        /// <exception cref="ArgumentNullException">Passed on in the return when the value is null</exception>
        public static Validation IsWithinRange<T>(this Validation validation, T? value, String name, T? min, T? max) where T : struct, IComparable
        {
            Validate.Begin()
                .IsNotNullOrEmpty(name, "name")
                .IsNotNull(min, "min")
                .IsNotNull(max, "max")
                .Check();

            if (min.Value.CompareTo(max.Value) > 0)
                throw new ArgumentException("min must be equal to or less than max");

            if (!value.HasValue)
                return validation.GetInstance().AddException(new ArgumentNullException(name, String.Format(ArgumentNull, name)));

            if (min.Value.CompareTo(value.Value) > 0 || max.Value.CompareTo(value.Value) < 0)
                return validation.GetInstance().AddException(new ArgumentOutOfRangeException(name, String.Format(ArgumentOutOfRange, name, value.Value, min.Value, max.Value)));

            return validation;
        }

        /// <summary>
        /// If the previous condition was met, checks a comparable value to ensure that it is within the specified range
        /// </summary>
        /// <typeparam name="T">Any object type which is a struct, and therefore cannot be null by itself, and which implements IComparable</typeparam>
        /// <param name="validation">The validation instance to carry forward, along with the result of the previous condition</param>
        /// <param name="value">An instance of T? to check</param>
        /// <param name="name">The name of the value</param>
        /// <param name="min">An instance of T? which acts as the minimum accepted value</param>
        /// <param name="max">An instance of T? which acts as the maximum accepted value</param>
        /// <exception cref="ArgumentException">Thrown if min is greater than max</exception>
        /// <exception cref="ValidationException">Thrown if min or max is null, or the name is an empty string</exception>
        /// <exception cref="ArgumentOutOfRange">Passed on in the return when the value is not within the expected range</exception>
        /// <exception cref="ArgumentNullException">Passed on in the return when the value is null</exception>
        public static Validation IsWithinRange<T>(this Tuple<Validation, Boolean> validation, T? value, String name, T? min, T? max) where T : struct, IComparable
        {
            if (validation.Item2)
                return validation.Item1.IsWithinRange(value, name, min, max);
            else
                return validation.Item1;
        }

        /// <summary>
        /// Checks a String value to ensure that it is not null or empty, and is a valid number as either an Int64 or Double value
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward</param>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null, passed on in the return when the value is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string, passed on in the return when the value is an empty string</exception>
        /// <exception cref="ArgumentOutOfRangeException">Passed on in the return when the value cannot be parsed as a valid Int64 or Double value</exception>
        public static Validation IsNumeric(this Validation validation, String value, String name)
        {
            Validate.Begin()
                .IsNotNullOrEmpty(name, "name")
                .Check();

            if (value == null)
                return validation.GetInstance().AddException(new ArgumentNullException(name, String.Format(ArgumentNull, name)));

            if (value == "")
                return validation.GetInstance().AddException(new ArgumentException(String.Format(ArgumentEmpty, name)));

            long a;
            double b;

            if (Int64.TryParse(value, out a))
                return validation;

            if (Double.TryParse(value, out b))
                return validation;

            return validation.GetInstance().AddException(new ArgumentOutOfRangeException(name, String.Format(ArgumentNotNumeric, name, value)));
        }

        /// <summary>
        /// If the previous condition was met, checks a String value to ensure that it is not null or empty, and is a valid number as either an Int64 or Double value
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward, along with the result of the previous condition</param>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null, passed on in the return when the value is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string, passed on in the return when the value is an empty string</exception>
        /// <exception cref="ArgumentOutOfRangeException">Passed on in the return when the value cannot be parsed as a valid Int64 or Double value</exception>
        public static Validation IsNumeric(this Tuple<Validation, Boolean> validation, String value, String name)
        {
            if (validation.Item2)
                return validation.Item1.IsNumeric(value, name);
            else
                return validation.Item1;
        }

        /// <summary>
        /// Checks a value or expression to ensure that it evaluates to true
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward</param>
        /// <param name="value">The value or expression to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentOutOfRangeException">Passed on in the return when the value or expression provided evaluates to false</exception>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string</exception>
        public static Validation IsTrue(this Validation validation, Boolean value, String name)
        {
            Validate.Begin()
                .IsNotNullOrEmpty(name, "name")
                .Check();

            if (value)
                return validation;
            else
                return validation.GetInstance().AddException(new ArgumentOutOfRangeException(name, String.Format(ArgumentBooleanWrong, name, true, value)));
        }

        /// <summary>
        /// If the previous condition was met, checks a value or expression to ensure that it evaluates to true
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward, along with the result of the previous condition</param>
        /// <param name="value">The value or expression to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentOutOfRangeException">Passed on in the return when the value or expression provided evaluates to false</exception>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string</exception>
        public static Validation IsTrue(this Tuple<Validation, Boolean> validation, Boolean value, String name)
        {
            if (validation.Item2)
                return validation.Item1.IsTrue(value, name);
            else
                return validation.Item1;
        }

        /// <summary>
        /// Checks a value or expression to ensure that it evaluates to false
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward</param>
        /// <param name="value">The value or expression to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentOutOfRangeException">Passed on in the return when the value or expression provided evaluates to true</exception>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string</exception>
        public static Validation IsFalse(this Validation validation, Boolean value, String name)
        {
            Validate.Begin()
                .IsNotNullOrEmpty(name, "name")
                .Check();

            if (value)
                return validation.GetInstance().AddException(new ArgumentOutOfRangeException(name, String.Format(ArgumentBooleanWrong, name, false, value)));
            else
                return validation;
        }

        /// <summary>
        /// If the previous condition was met, checks a value or expression to ensure that it evaluates to false
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward, along with the result of the previous condition</param>
        /// <param name="value">The value or expression to check</param>
        /// <param name="name">The name of the value</param>
        /// <exception cref="ArgumentOutOfRangeException">Passed on in the return when the value or expression provided evaluates to true</exception>
        /// <exception cref="ArgumentNullException">Thrown when the name provided is null</exception>
        /// <exception cref="ArgumentException">Thrown when the name provided is an empty string</exception>
        public static Validation IsFalse(this Tuple<Validation, Boolean> validation, Boolean value, String name)
        {
            if (validation.Item2)
                return validation.Item1.IsFalse(value, name);
            else
                return validation.Item1;
        }

        /// <summary>
        /// Provide a condition to determine whether or not the next validation should occur
        /// </summary>
        /// <param name="validation">The Validation instance to carry forward</param>
        /// <param name="condition">The condition that must be true for the next validation step to occur</param>
        public static Tuple<Validation, Boolean> ValidateWhen(this Validation validation, Boolean condition)
        {
            return Tuple.Create(validation, condition);
        }

        /// <summary>
        /// Creates an instance of <seealso cref="Validation"/> if the given value is null
        /// </summary>
        internal static Validation GetInstance(this Validation validation)
        {
            if (validation == null)
                return new Validation();
            else
                return validation;
        }

        /// <summary>
        /// Checks a Validation instance for collected exceptions, throwing an ValidationException wrapper if any are available
        /// </summary>
        public static Validation Check(this Validation validation)
        {
            if (validation == null)
                return validation;
            else
            {
                validation.ObserveExceptions();
                return validation;
            }
        }
    }
}
