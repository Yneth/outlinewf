namespace OutlineWF.Utilities
{
    public static class IntegerExtension
    {
        public static bool IsInRange(this int number, int low, int high)
        {
            return number >= low && number <= high;
        }

        public static bool IsInRangeExclusive(this int number, int low, int high)
        {
            return number > low && number < high;
        }

        public static bool IsNegative(this int number)
        {
            return number < 0;
        }

        public static bool IsPositive(this int number)
        {
            return number > 0;
        }

        public static string ToString(this int number)
        {
            return number.ToString("N");
        }
    }
}
