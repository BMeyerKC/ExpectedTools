using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.EnumerableExtensions
{
    [TestClass]
    public class ToCommaSeperatedString
    {
        [TestMethod]
        public void CanHandleASimpleArray()
        {
            var testMe = new string[2] { "1", "2" };

            var result = testMe.ToCommaSeperatedString();

            const string expected = "1,2";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CanHandleNullEnum()
        {
            string[] testMe = null;

            var result = testMe.ToCommaSeperatedString();

            const string expected = "";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CanHandleNullComplexTypes()
        {
            var testMe = new List<TestObject>();

            var result = testMe.ToCommaSeperatedString();

            const string expected = "";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CanHandleComplexTypes()
        {
            var testMe = new List<TestObject>();
            testMe.Add(new TestObject
            {
                Name = "Mark"
            });
            var result = testMe.ToCommaSeperatedString();

            const string expected = "Name,Mark";
            Assert.AreEqual(expected, result);
        }

        public class TestObject
        {
            public string Name { get; set; }
        }
    }
}