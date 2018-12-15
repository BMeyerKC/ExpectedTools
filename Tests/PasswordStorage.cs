using ExpectedTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.PasswordStorageTest
{
    [TestClass]
    public class PasswordHash
    {
        [TestMethod]
        public void PasswordHashCreatesAString()
        {
            const string testPassword = "9ddxXgoXCPpvr4Vi";
            var hashedPassword = PasswordStorage.CreateHash(testPassword);

            Assert.AreNotEqual(testPassword, hashedPassword);
        }

        [TestMethod]
        public void PasswordHashCanBeVerified()
        {
            const string testPassword = "Y6uwns7ipyi3Xdfs";
            var hashedPassword = PasswordStorage.CreateHash(testPassword);

            Assert.AreNotEqual(testPassword, hashedPassword);

            var passwordIsCorrect = PasswordStorage.VerifyPassword(testPassword, hashedPassword);
            Assert.IsTrue(passwordIsCorrect);
        }
    }
}