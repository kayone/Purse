using NUnit.Framework;

namespace Purse.Tests
{
    public abstract class BaseTest
    {
        protected Counter Counter { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Counter = new Counter();
        }
    }
}