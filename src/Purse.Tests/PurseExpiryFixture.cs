﻿using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace Purse.Tests
{
    [TestFixture]
    public class PurseExpiryFixture : BaseTest
    {
        private Purse<string, string> _cachedString = new Purse<string, string>();

        [SetUp]
        public void SetUp()
        {
            _cachedString = new Purse<string, string>();
        }

        [Test]
        public void should_not_expire_before_ttl()
        {
            _cachedString.Get("Test", Counter.GetString, TimeSpan.FromSeconds(5));
            _cachedString.Get("Test", Counter.GetString, TimeSpan.FromSeconds(5));

            Counter.HitCount.Should().Be(1);
        }


        [Test]
        public void find_should_return_default_if_expired()
        {
            _cachedString.Get("Test", Counter.GetString, TimeSpan.FromMilliseconds(50));

            _cachedString.Find("Test").Should().NotBeNull();

            Thread.Sleep(100);

            _cachedString.Find("Test").Should().BeNull();

            Counter.HitCount.Should().Be(1);
        }


        [Test]
        public void should_honor_ttl()
        {
            int hitCount = 0;
            _cachedString = new Purse<string, string>();

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