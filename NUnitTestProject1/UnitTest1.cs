using NUnit.Framework;

namespace NUnitTestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var result = 2 + 2 * 3;
            Assert.AreEqual(8, result);
        }
    }
}