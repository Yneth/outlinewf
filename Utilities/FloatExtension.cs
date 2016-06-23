namespace OutlineWF.Utilities
{
    public static class FloatExtension
    {
        public static bool IsInRange(this float number, float low, float high)
        {
            return number >= low && number <= high;
        }

        public static bool IsInRangeExclusive(this float number, float low, float high)
        {
            return number > low && number < high;
        }

        public static bool IsNegative(this float number)
        {
            return number < 0;
        }

        public static bool IsPositive(this float number)
        {
            return number > 0;
        }

        public static string ToString(this float number)
        {
            return number.ToString("N");
        }
    }
}
