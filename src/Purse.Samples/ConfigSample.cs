using System;

namespace Purse.Samples
{
    public class ThrottleSample
    {
        public static void Main()
        {
            var _cache = new Cache<string, DateTime>();

            while (true)
            {
                _cache.Get("Time", MethodThatShouldNotBeCalledVeryFrequently, TimeSpan.FromSeconds(2));
            }

        }

        private static DateTime MethodThatShouldNotBeCalledVeryFrequently()
        {
            var time = DateTime.Now;
            Console.WriteLine(time);
            return time;
        }

        //OUTPUT
        //2014-01-13 10:00:14 PM
        //2014-01-13 10:00:16 PM
        //2014-01-13 10:00:18 PM
        //2014-01-13 10:00:20 PM
    }
}
