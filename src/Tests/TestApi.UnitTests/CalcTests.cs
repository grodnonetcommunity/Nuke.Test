using NUnit.Framework;

namespace TestApi.UnitTests
{
    public class CalcTests
    {
        [TestCase(1, 2, 3)]
        [TestCase(40, 2, 42)]
        public void AddTwoNumbers(int a, int b, int result)
        {
            Assert.That(result, Is.EqualTo(a + b));
        }
    }
}
