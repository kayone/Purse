using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace Purse.Tests
{
    [TestFixture]
    public class PrefetchedCacheFixture
    {
        int _fetchCount = 0;

        [SetUp]
        public void SetUp()
        {
            _fetchCount = 0;
        }

        [Test]
        public void should_populate_cache_on_creation()
        {
            var cache = new PrefetchedCache<string, int>(OnFetchFunc);
            cache.Count.Should().Be(2);
        }


        [Test]
        public void should_respect_long_expiry()
        {
            var cache = new PrefetchedCache<string, int>(OnFetchFunc, TimeSpan.FromHours(1));
            cache.Get("a");
            cache.Get("a");
            cache.Get("a");

            _fetchCount.Should().Be(1);
        }

        [Test]
        public void should_respect_no_expiry()
        {
            var cache = new PrefetchedCache<string, int>(OnFetchFunc);
            cache.Get("a");
            cache.Get("a");
            cache.Get("a");

            _fetchCount.Should().Be(1);
        }


        [Test]
        public void should_allow_manual_refresh_with_no_expiry()
        {
            var cache = new PrefetchedCache<string, int>(OnFetchFunc);
            cache.Get("a");
            cache.Get("a");
            cache.Get("a");
            _fetchCount.Should().Be(1);
            cache.Refresh();
            _fetchCount.Should().Be(2);
        
        }

        [Test]
        public void should_repopulate_cache_on_expiry()
        {
            var cache = new PrefetchedCache<string, int>(OnFetchFunc, TimeSpan.FromSeconds(1));
            cache.Count.Should().Be(2);

            Thread.Sleep(1000);

            cache.Get("a");
            cache.Get("a");
            cache.Get("a");

            _fetchCount.Should().Be(2);

        }

        private Dictionary<string, int> OnFetchFunc()
        {
            _fetchCount++;
            return new Dictionary<string, int>
            {
                {"a", 1}, {"b", 2}
            };
        }
    }
}