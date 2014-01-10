namespace Purse.Tests
{
    public class Counter
    {
        public int HitCount { get; private set; }

        public string GetString()
        {
            HitCount++;
            return "Hit count is " + HitCount;
        }
    }
}