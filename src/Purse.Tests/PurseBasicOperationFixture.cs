using FluentAssertions;
using NUnit.Framework;

namespace Purse.Tests
{
    [TestFixture]
    public class PurseBasicOperationFixture
    {
        private Cache<string, string> _cachedString = new Cache<string, string>();
        private Worker _worker;

        [SetUp]
        public void SetUp()
        {
            _cachedString = new Cache<string, string>();
            _worker = new Worker();
        }

        [Test]
        public void should_call_function_once()
        {
            _cachedString.Get("Test", _worker.GetString);
            _cachedString.Get("Test", _worker.GetString);

            _worker.HitCount.Should().Be(1);
        }

        [Test]
        public void multiple_calls_should_return_same_result()
        {
            var first = _cachedString.Get("Test", _worker.GetString);
            var second = _cachedString.Get("Test", _worker.GetString);

            first.Should().Be(second);
        }


        [Test]
        public void should_be_able_to_update_key()
        {
            _cachedString.Add("Key", "Old");
            _cachedString.Add("Key", "New");

            _cachedString.Get("Key").Should().Be("New");
        }


        [Test]
        public void should_be_able_to_remove_key()
        {
            _cachedString.Add("Key", "Value");

            _cachedString.Remove("Key");

            _cachedString.Get("Key").Should().BeNull();
        }

        [Test]
        public void should_be_able_to_remove_non_existing_key()
        {
            _cachedString.Remove("Key");
        }

        [Test]
        public void should_store_null()
        {
            int hitCount = 0;


            for (int i = 0; i < 10; i++)
            {
                _cachedString.Get("key", () =>
                {
                    hitCount++;
                    return null;
                });
            }

            hitCount.Should().Be(1);
        }


        [Test]
        public void should_return_count()
        {
            _cachedString.Add("Key1", "Value1");
            _cachedString.Add("Key2", "Value2");
            _cachedString.Add("Key3", "Value3");

            _cachedString.Count.Should().Be(3);
        }

        [Test]
        public void purge_should_clear_all_items()
        {
            _cachedString.Add("Key1", "Value1");
            _cachedString.Add("Key2", "Value2");
            _cachedString.Add("Key3", "Value3");

            _cachedString.Count.Should().Be(3);

            _cachedString.Purge();

            _cachedString.Values.Should().BeEmpty();
        }


        [Test]
        public void should_return_values()
        {
            _cachedString.Add("Key1", "Value1");
            _cachedString.Add("Key2", "Value2");
            _cachedString.Add("Key3", "Value3");

            _cachedString.Values.Should().HaveCount(3);
        }

        private class Worker
        {
            public int HitCount { get; private set; }

            public string GetString()
            {
                HitCount++;
                return "Hit count is " + HitCount;
            }
        }
    }
}