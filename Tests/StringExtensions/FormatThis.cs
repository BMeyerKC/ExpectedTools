using System;
using NUnit.Framework;

namespace Tests.StringExtensions
{
    [TestFixture]
    public class FormatThis
    {
        [Test]
        public void MakesIdenticalCallToStringFormat()
        {
            const string addme = "{0} End";

            var result = addme.FormatThis("Start");

            Assert.AreEqual("Start End", result);
        }
    }

    [TestFixture]
    public class NullOrEmpty
    {
        [Test]
        public void ReturnTrueIfEmpty()
        {
            const string emptyString = "";

            var result = emptyString.NullOrEmpty();

            Assert.IsTrue(result);
        }

        [Test]
        public void ReturnTrueIfNull()
        {
            string nullString = null;

            var result = nullString.NullOrEmpty();

            Assert.IsTrue(result);
        }

        [Test]
        public void ReturnsFalseIfNotEmpty()
        {
            const string realString = "IHazValue";

            var result = realString.NullOrEmpty();

            Assert.IsFalse(result);
        }
    }
}
