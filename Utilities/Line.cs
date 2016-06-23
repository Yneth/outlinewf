using System;
using System.Xml.Schema;

namespace OutlineWF.Utilities
{
    public class Line
    {
        private readonly Point _p0;
        private readonly Point _p1;

        private readonly float _a;
        private readonly float _b;
        private readonly float _c;
        private readonly float _k;

        private float? _magnitude;

        public float A { get { return _a; } }
        public float B { get { return _b; } }
        public float C { get { return _c; } }

        public float K { get { return _k; } }

        public Point P0 { get { return _p0; } }
        public Point P1 { get { return _p1; } }

        public float Magnitude
        {
            get
            {
                if (!_magnitude.HasValue) { _magnitude = P0.DistanceTo(P1); }
                return _magnitude.Value;
            }
        }

        public Line(Point p0, Point p1)
        {
            _p0 = p0;
            _p1 = p1;

            _a = P1.Y - P0.Y;
            _b = P0.X - P1.X;
            _c = P1.X * P0.Y - P0.X * P1.Y;

            if (Math.Abs(_b) < 0.001) _k = float.NaN;
            else _k = -_a / _b;
        }

        public float Length()
        {
            return _p0.DistanceTo(_p1);
        }

        public float DistanceTo(Point p)
        {
            return (float)(Math.Abs(A * p.X + B * p.Y + C) / Math.Sqrt(A * A + B * B));
        }

        public Line Perpendicular(Point p)
        {
            return new Line(p, ClosestPoint(p));
        }

        public bool Contains(Point p)
        {
            var t1 = Math.Min(P0.X, P1.X);
            var t2 = Math.Max(P0.X, P1.X);
            var q1 = Math.Min(P0.Y, P1.Y);
            var q2 = Math.Max(P0.Y, P1.Y);
            return t1 <= p.X && p.X <= t2 && q1 <= p.Y && p.Y <= q2;
        }

        public Point ClosestPoint(Point p)
        {
            const double tolerance = 0.001;
            if (Math.Abs(A) < tolerance) return new Point(p.X, P1.Y);
            if (Math.Abs(B) < tolerance) return new Point(P1.X, p.Y);

            //var k = (P1.X - P0.X) / (P1.Y - P0.Y);
            //var k = (P0.X - P1.X) / (P0.Y - P1.Y);
            //var k = -B / A;
            var k = -1 / K;
            var x = (k * B * p.X - B * p.Y - C) / (A + k * B);
            var y = p.Y + k * (x - p.X);
            //var x = -(p.Y * B + C) / A;
            //var y = -(A * p.X) / C;
            return new Point(x, y);
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            var that = obj as Line;

            if (that == null) return false;
            if (!that.P0.Equals(P0)) return false;
            if (!that.P1.Equals(P1)) return false;

            return true;
        }
    }
}
