using System;
using System.Text;

namespace OutlineWF.Utilities
{
    public class Point
    {
        #region Fields
        private readonly float _x;
        private readonly float _y;
        #endregion

        #region Properties
        public float X { get { return _x; } }
        public float Y { get { return _y; } }
        #endregion

        #region Public Methods
        public Point(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(_x * _x + _y * _y);
        }

        public float DistanceTo(Point p)
        {
            return (float)Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
        }

        public static Point Lerp(Point a0, Point a1, Point a2, Point a3, float t)
        {
            return ((a3 * t + a2) * t + a1) * t + a0;
        }

        public static Point operator *(Point p1, Point p2)
        {
            return p1.Multiply(p2);
        }

        public static Point operator *(Point p1, float num)
        {
            return new Point(p1.X * num, p1.Y * num);
        }

        public static Point operator /(Point p1, float num)
        {
            return new Point(p1.X / num, p1.Y / num);
        }

        public static Point operator /(Point p1, Point p2)
        {
            return p1.Divide(p2);
        }

        public static Point operator +(Point p1, float num)
        {
            return new Point(p1.X + num, p1.Y + num);
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, float num)
        {
            return p1 + (-num);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return p1 + (-p2);
        }

        public static Point operator -(Point p1)
        {
            return new Point(-p1.X, -p1.Y);
        }
        #endregion

        #region Object
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point)obj);
        }

        protected bool Equals(Point other)
        {
            const double eps = 0.001;
            if (Math.Abs(other.X - this.X) > eps) return false;
            if (Math.Abs(other.Y - this.Y) > eps) return false;
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_x.GetHashCode() * 397) ^ _y.GetHashCode();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            return sb.Append(X).Append("    ").Append(Y).ToString();
        }
        #endregion
    }
}
