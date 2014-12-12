using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace Purse.Tests
{
    [TestFixture]
    public class PurseExpiryFixture : BaseTest
    {
        private Cache<string, string> _cachedString = new Cache<string, string>();

        [SetUp]
        public void SetUp()
        {
            _cachedString = new Cache<string, string>();
        }

        [Test]
        public void should_not_expire_before_ttl()
        {
            _cachedString.Get("Test", Counter.GetString, TimeSpan.FromSeconds(5));
            _cachedString.Get("Test", Counter.GetString, TimeSpan.FromSeconds(5));

            Counter.HitCount.Should().Be(1);
        }


        [Test]
        public void purgeexpired_should_purge_expired_items()
        {
            _cachedString.Set("Key1", "Value1", TimeSpan.FromMilliseconds(1));
            _cachedString.Set("Key2", "Value1", TimeSpan.FromMilliseconds(1));
            _cachedString.Set("Key3", "Value1", TimeSpan.FromMilliseconds(1));

            Thread.Sleep(100);

            _cachedString.PurgeExpired();

            _cachedString.Values.Should().BeEmpty();
        }

        [Test]
        public void purgeexpired_should_not_purge_nonexpired_items()
        {
            _cachedString.Set("Key1", "Value1", TimeSpan.FromMinutes(1));
            _cachedString.Set("Key2", "Value1", TimeSpan.FromMinutes(1));
            _cachedString.Set("Key3", "Value1", TimeSpan.FromMinutes(1));

            _cachedString.PurgeExpired();

            _cachedString.Values.Should().HaveCount(3);
        }

        [Test]
        public void purgeexpired_should_not_purge_items_with_no_ttl_items()
        {
            _cachedString.Set("Key1", "Value1");
            _cachedString.Set("Key2", "Value1");
            _cachedString.Set("Key3", "Value1");

            _cachedString.PurgeExpired();

            _cachedString.Values.Should().HaveCount(3);
        }


        [Test]
        public void get_should_throw_exception_if_expired()
        {
            _cachedString.Get("Test", Counter.GetString, TimeSpan.FromMilliseconds(50));

            _cachedString.Get("Test").Should().NotBeNull();

            Thread.Sleep(100);

            Assert.Throws<CacheKeyNotFoundException>(() => _cachedString.Get("Test"));
        }

        [Test]
        public void get_should_throw_if_missing_key()
        {
            Assert.Throws<CacheKeyNotFoundException>(() => _cachedString.Get("UnknownKey"));
        }


        [Test]
        public void should_honor_ttl()
        {
            int hitCount = 0;
            _cachedString = new Cache<string, string>();

            for (int i = 0; i < 100; i++)
            {
                _cachedString.Get("key", () =>
                {
                    hitCount++;
                    return null;
                }, TimeSpan.FromMilliseconds(300));

                Thread.Sleep(10);
            }

            hitCount.Should().BeInRange(3, 6);
        }


    }
}