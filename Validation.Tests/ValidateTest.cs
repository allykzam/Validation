using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validation;

namespace Validation.Tests
{
    [TestClass]
    public class ValidateTest
    {
        [TestMethod]
        public void BeginShouldReturnNull()
        {
            Assert.IsTrue(Validate.Begin() == null);
        }
    }
}
