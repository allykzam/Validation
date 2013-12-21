using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validation;
using System.Linq;
using MSTestExtensions;

namespace Validation.Tests
{
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public void ExceptionsShouldInitializeEmpty()
        {
            var valid = new Validation();

            Assert.IsTrue(valid.Exceptions.Count() == 0);
        }

        [TestMethod]
        public void AddSingleException()
        {
            var valid = new Validation();

            valid = valid.AddException(new NullReferenceException());

            Assert.IsTrue(valid.Exceptions.Count() == 1);
            ExceptionAssert.Throws<ValidationException>(() => valid.Check());
        }
    }
}
