using System;

namespace OutlineWF.Utilities
{
    public class Ray
    {
        private readonly Point _point;
        private readonly float _k; // K in radians

        public Point Point
        {
            get { return _point; }
        }

        public float K
        {
            get { return _k; }
        }

        public Ray(Point p, float k)
        {
            _point = p;
            _k = k;
        }

        public Point GetPointFromStart(double dist)
        {
            float degree = (float)Math.Atan(_k);
            float x = (float)(dist * Math.Cos(degree));
            float y = (float)(dist * Math.Sin(degree));
            return new Point(_point.X + x, _point.Y + y);
        }
    }
}
