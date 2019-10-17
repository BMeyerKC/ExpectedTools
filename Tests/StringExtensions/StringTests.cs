using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.StringExtensions
{
    [TestClass]
    public class FormatThis
    {
        [TestMethod]
        public void MakesIdenticalCallToStringFormat()
        {
            const string addme = "{0} End";

            var result = addme.FormatThis("Start");

            Assert.AreEqual("Start End", result);
        }
    }

    [TestClass]
    public class RemoveFromEnd
    {
        [TestMethod]
        public void StringIsRemovedFromEnd()
        {
            const string addme = "MonkeyBarFoo";

            var result = addme.RemoveFromEnd("Foo");

            Assert.AreEqual("MonkeyBar", result);
        }
    }

    [TestClass]
    public class NullOrEmpty
    {
        [TestMethod]
        public void ReturnTrueIfEmpty()
        {
            const string emptyString = "";

            var result = emptyString.NullOrEmpty();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnTrueIfNull()
        {
            string nullString = null;

            var result = nullString.NullOrEmpty();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsFalseIfNotEmpty()
        {
            const string realString = "IHazValue";

            var result = realString.NullOrEmpty();

            Assert.IsFalse(result);
        }
    }
}