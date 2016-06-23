using System;

namespace OutlineWF.Utilities
{
    public static class PointExtension
    {
        public static Point Multiply(this Point p1, Point p2)
        {
            return new Point(p1.X * p2.X, p1.Y * p2.Y);
        }

        public static Point Divide(this Point p1, Point p2)
        {
            return new Point(p1.X / p2.X, p1.Y / p2.Y);
        }

        public static float SquareDistanceTo(this Point p1, Point p2)
        {
            return (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
        }

        public static float DistanceTo(this Point p1, Point p2)
        {
            return (float)Math.Sqrt(p1.SquareDistanceTo(p2));
        }
    }
}
