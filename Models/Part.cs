using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using OutlineWF.Utilities;
using OutlineWF.Views;
using Point = OutlineWF.Utilities.Point;

namespace OutlineWF.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Part
    {
        #region Fields
        private List<Point> _vertices;
        #endregion

        #region Properties
        public string Name { get; set; }
        public Point Center { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float MaxX { get; private set; }
        public float MinX { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }
        public float Square { get; private set; }
        public float Perimeter { get; private set; }

        public List<Point> Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }
        #endregion

        #region Public Methods
        public Part()
        {
            _vertices = new List<Point>();
            MinX = float.MaxValue;
            MaxX = float.MinValue;
            MinY = float.MaxValue;
            MaxY = float.MinValue;
        }

        public Part(string name) : this()
        {
            Name = name;
        }

        public Part(List<Point> points) : this()
        {
            _vertices = points;
            Rescale();
        }

        public void AddPoint(Point position)
        {
            _vertices.Add(position);
            Rescale();
        }

        public void AddPoint(Point mouse, int width, int height)
        {
            var mxy = (float)CalculateMaxScale(width, height);
            var real = new Point(
                Center.X + (mouse.X - width / 2.0f) / mxy,
                Center.Y - (height / 2.0f - mouse.Y) / mxy
                );
            var index = IndexOfClosestLineTo(real);
            var line = GetLine(index);
            var point = line.ClosestPoint(real);
            _vertices.Insert(IncrementIndex(index), point);
            Rescale();
        }

        public void Remove(Point point)
        {
            _vertices.Remove(ClosestPointTo(point));
            Rescale();
        }

        public void Remove(Point position, int w, int h)
        {
            Remove(ToLocalCoordinates(position, w, h));
            Rescale();
        }

        public void Set(Point old, Point newValue, int w, int h)
        {
            var realNew = ToLocalCoordinates(newValue, w, h);
            var indexOfOld = IndexOfClosestLocalPoint(old, w, h);
            _vertices[indexOfOld] = realNew;
            Rescale();
        }

        public Point GetPointOnPart(Point mouse, int width, int height)
        {
            var mxy = (float)CalculateMaxScale(width, height);
            var real = new Point(
                Center.X + (mouse.X - width / 2.0f) / mxy,
                Center.Y - (height / 2.0f - mouse.Y) / mxy
                );
            var index = IndexOfClosestLineTo(real);
            var line = GetLine(index);
            return line.ClosestPoint(real);
        }

        public void Compress(float tolerance = 0.05f)
        {
            if (_vertices == null || _vertices.Count == 0)
            {
                return;
            }

            var newVertices = new HashSet<Point> { _vertices[0] };
            for (int i = 0, k = i + 2; k < _vertices.Count; k++)
            {
                var line = new Line(_vertices[i], _vertices[k]);
                for (var j = i + 1; j < k; j++)
                {
                    if (line.DistanceTo(_vertices[j]) >= tolerance)
                    {
                        i = k - 1;
                        newVertices.Add(_vertices[i]);
                    }
                }
            }
            newVertices.Add(_vertices[_vertices.Count - 1]);
            _vertices = newVertices.ToList();
            Rescale();
        }

        public List<Point> GetEquidistant(int start, int end, float length, bool loop = false)
        {
            var result = new List<Point>(_vertices.Count);
            foreach (var line in LineSectionIterator(start, end, loop))
            {
                var k = -1.0f / line.K;

                Point p0, p1;
                var r0 = new Ray(line.P0, k);
                var r1 = new Ray(line.P1, k);

                if (r0.GetPointFromStart(-length).DistanceTo(Center) > r0.GetPointFromStart(length).DistanceTo(Center))
                {
                    p0 = r0.GetPointFromStart(-length);
                }
                else
                {
                    p0 = r0.GetPointFromStart(length);
                }

                if (r1.GetPointFromStart(-length).DistanceTo(Center) > r1.GetPointFromStart(length).DistanceTo(Center))
                {
                    p1 = r1.GetPointFromStart(-length);
                }
                else
                {
                    p1 = r1.GetPointFromStart(length);
                }

                result.Add(p0);
                result.Add(p1);
            }
            return result;
        }

        public Part SerialGraduation(int sSize, int iSize, List<Amplifier> amplifiers)
        {
            var v = _vertices;

            var newVerts = new List<Point>
            {
                v[amplifiers[0].Id] + amplifiers[0].Delta*(sSize - iSize)/2
            };
            for (var i = 0; i < amplifiers.Count - 1; i++)
            {
                var s = amplifiers[i];
                var e = amplifiers[i + 1];

                var sP = v[s.Id] + s.Delta * (sSize - iSize) / 2;
                var eP = v[e.Id] + e.Delta * (sSize - iSize) / 2;

                var denom = (float)(Math.Pow(v[e.Id].X - v[s.Id].X, 2) + Math.Pow(v[e.Id].Y - v[s.Id].Y, 2));
                var p = ((eP.X - sP.X) * (v[e.Id].X - v[s.Id].X) + (eP.Y - sP.Y) * (v[e.Id].Y - v[s.Id].Y)) / denom;
                var q = -((eP.X - sP.X) * (v[e.Id].Y - v[s.Id].Y) - (eP.Y - sP.Y) * (v[e.Id].X - v[s.Id].X)) / denom;

                var loop = i == amplifiers.Count - 2;
                foreach (var point in SectionIterator(s.Id + 1, e.Id - 1, loop))
                {
                    var resX = sP.X + (point.X - v[s.Id].X) * p - (point.Y - v[s.Id].Y) * q;
                    var resY = sP.Y + (point.X - v[s.Id].X) * q + (point.Y - v[s.Id].Y) * p;
                    newVerts.Add(new Point(resX, resY));
                }
                if (!loop) { newVerts.Add(eP); }
            }
            return new Part(newVerts);
        }

        public void CoupleAngleAt(Point point, int w, int h, float ras, float eps = 0.001f)
        {
            CoupleAngleAt(ToLocalCoordinates(point, w, h), ras, eps);
        }

        public void CoupleAngleAt(Point point, float dist, float eps = 0.001f)
        {
            var ai = IndexOfClosestPoint(point);
            var aim1 = DecrementIndex(ai);
            var aip1 = IncrementIndex(ai);

            var c = _vertices[aim1];
            var a = _vertices[ai];
            var b = _vertices[aip1];

            var ac = new Line(a, c);
            var ab = new Line(a, b);

            var isBigger = ac.Magnitude > ab.Magnitude;
            SwapIf(ref aim1, ref aip1, (l0, l1) => isBigger);
            SwapIf(ref ac, ref ab, (l0, l1) => isBigger);
            SwapIf(ref c, ref b, (l0, l1) => isBigger);

            var d0 = dist > ac.Magnitude ? ac.Magnitude : dist;
            var cs = (c - a) * (d0 / ac.Magnitude) + a;
            var bs = (b - a) * (d0 / ab.Magnitude) + a;

            var d = (cs + bs) / 2;

            var ad = new Line(a, d);

            var aoMagnitude = a.SquareDistanceTo(cs) / ad.Magnitude;
            var o = (d - a) * (aoMagnitude / ad.Magnitude) + a;
            var co = o.DistanceTo(cs);

            var phi = Math.Acos((cs.SquareDistanceTo(bs) - co * co - o.SquareDistanceTo(bs)) / (-2 * co * o.DistanceTo(bs)));

            var n = (int)(phi / (2 * (float)Math.Acos((co - eps) / co)));

            var oc = co * (float)Math.Cos(phi / (2 * n));

            var phiN = phi / n;
            //var phiN = 10.0f; // lol
            var points = new List<Point>(n + 1);
            for (var k = 1; k < n; k++)
            {
                var angle = k * phiN;
                var cos = Math.Cos(angle);
                var sin = Math.Sin(angle);

                var xk = bs.X * cos - bs.Y * sin - o.X * cos + o.Y * sin + o.X;
                var yk = bs.X * sin + bs.Y * cos - o.X * sin - o.Y * cos + o.Y;
                points.Add(new Point((float)xk, (float)yk));
            }

            var from = (aim1 > aip1 ? aip1 : aim1) + 1;
            _vertices.RemoveRange(from, Math.Abs(aip1 - aim1));
            _vertices.InsertRange(from, points);
            Rescale();
        }

        public void BSplineSmoothRange(Point from, Point to, int pointCount = 10)
        {
            SmoothRange(from, to, BasicSpline, pointCount);
        }

        public void BSplineSmoothRange(Point from, Point to, int w, int h, int pointCount = 10)
        {
            if (from == null || to == null)
            {
                return;
            }
            BSplineSmoothRange(ToLocalCoordinates(from, w, h), ToLocalCoordinates(to, w, h), pointCount);
        }

        public void InterSplineSmoothRange(Point from, Point to, int w, int h, int pointCount = 10)
        {
            if (from == null || to == null)
            {
                return;
            }
            InterSplineSmoothRange(ToLocalCoordinates(from, w, h), ToLocalCoordinates(to, w, h), pointCount);
        }

        public void InterSplineSmoothRange(Point from, Point to, int pointCount = 10)
        {
            SmoothRange(from, to, InterSpline, pointCount);
        }

        public IEnumerable<Line> LineSectionIterator(int start, int end, bool loop = false)
        {
            var s = start;
            var e = end;
            if (!loop)
            {
                SwapIf(ref s, ref e, (i, i1) => start > end);
            }
            else
            {
                SwapIf(ref s, ref e, (i, i1) => start < end);
            }

            for (int i = s; i != DecrementIndex(e); i = IncrementIndex(i))
            {
                yield return GetLine(i);
            }
            yield return new Line(_vertices[DecrementIndex(e)], _vertices[e]);
        }

        public IEnumerable<Point> SectionIterator(int start, int end, bool loop = false)
        {
            if (!loop)
            {
                SwapIf(ref start, ref end, (i, i1) => start > end);
            }
            else
            {
                SwapIf(ref start, ref end, (i, i1) => start < end);
            }

            for (int i = start; i != end; i = IncrementIndex(i))
            {
                yield return _vertices[i];
            }
            yield return _vertices[end];
        }

        public IEnumerable<Point> Range(Point from, Point to, bool toInclusive = true)
        {
            var fromIndex = IndexOfClosestPoint(from);
            for (var i = fromIndex; !_vertices[i].Equals(to); i = IncrementIndex(i))
            {
                yield return _vertices[i];
            }
            if (toInclusive)
            {
                yield return to;
            }
        }

        public int IndexOfClosestPoint(Point position)
        {
            var index = -1;
            var closestDistance = double.MaxValue;
            for (var i = 0; i < _vertices.Count; i++)
            {
                var dst = _vertices[i].SquareDistanceTo(position);
                if (closestDistance > dst)
                {
                    closestDistance = dst;
                    index = i;
                }
            }
            return index;
        }

        public int IndexOfClosestLocalPoint(Point position, int w, int h)
        {
            return IndexOfClosestPoint(ToLocalCoordinates(position, w, h));
        }

        public Point ClosestPointTo(Point point)
        {
            return _vertices[IndexOfClosestPoint(point)];
        }

        public Point ClosestLocalPointTo(Point position, int w, int h)
        {
            return _vertices[IndexOfClosestLocalPoint(position, w, h)];
        }

        public int IndexOfClosestLineTo(Point p)
        {
            var index = 0;
            var minDistance = double.MaxValue;
            for (var i = 0; i < _vertices.Count; i++)
            {
                var line = new Line(_vertices[i], _vertices[IncrementIndex(i)]);

                var dst = line.DistanceTo(p);
                if (minDistance > dst && line.Contains(line.ClosestPoint(p)))
                {
                    minDistance = dst;
                    index = i;
                }
            }
            return index;
        }

        public int IndexOfClosestLineToReal(Point p, int w, int h)
        {
            return IndexOfClosestLineTo(ToLocalCoordinates(p, w, h));
        }

        public double CalculateMaxScale(int width, int height)
        {
            return Math.Min(width / Width, height / Height);
        }

        public Point ToLocalCoordinates(Point p, int width, int height)
        {
            var mxy = (float)CalculateMaxScale(width, height);
            return new Point(
                Center.X + (p.X - width / 2.0f) / mxy,
                Center.Y - (height / 2.0f - p.Y) / mxy
                );
        }

        public Point ToScreenCoordinates(Point real, int w, int h)
        {
            var mxy = CalculateMaxScale(w, h);
            var x = (real.X - Center.X) * mxy + w / 2.0f;
            var y = (Center.Y - real.Y) * mxy + h / 2.0f;
            return new Point((float)x, (float)y);
        }

        #region Drawing
        public void Draw(Graphics graphics, PartDrawContext context)
        {
            if (_vertices.Count <= 0)
            {
                return;
            }

            int width = context.Width;
            int height = context.Height;

            if (_vertices.Count == 1)
            {
                DrawPoint(graphics, context.Color, ToScreenCoordinates(_vertices[0], width, height));
                return;
            }

            for (var i = 0; i < _vertices.Count - 1; i++)
            {
                var p0 = ToScreenCoordinates(_vertices[i], width, height);
                var p1 = ToScreenCoordinates(_vertices[i + 1], width, height);
                DrawPoint(graphics, context.Color, p0);
                DrawPoint(graphics, context.Color, p1);
                DrawLine(graphics, context.Color, p0, p1);
            }
            var end = ToScreenCoordinates(_vertices[_vertices.Count - 1], width, height);
            var start = ToScreenCoordinates(_vertices[0], width, height);

            DrawLine(graphics, context.Color, end, start);
        }

        public void Draw(Graphics graphics, PartDrawContext context, Point center, double mxy)
        {
            if (_vertices.Count <= 0)
            {
                return;
            }

            int w = context.Width;
            int h = context.Height;

            var realPoints = _vertices.Select(point => new Point(
                    (float)((point.X - center.X) * mxy + w / 2.0f),
                    (float)((center.Y - point.Y) * mxy + h / 2.0f)
            )).ToList();

            if (_vertices.Count == 1)
            {
                DrawPoint(graphics, context.Color, realPoints[0]);
                return;
            }

            for (var i = 0; i < _vertices.Count - 1; i++)
            {
                var p0 = realPoints[i];
                var p1 = realPoints[i + 1];
                DrawPoint(graphics, context.Color, p0);
                DrawPoint(graphics, context.Color, p1);
                DrawLine(graphics, context.Color, p0, p1);
            }
            var end = realPoints[_vertices.Count - 1];
            var start = realPoints[0];

            DrawLine(graphics, context.Color, end, start);
        }

        public void DrawPoint(Graphics graphics, Color color, Point position)
        {
            const int width = 5;
            const int height = 5;
            using (var brush = new SolidBrush(color))
            {
                graphics.FillEllipse(brush,
                    position.X - width / 2.0f, position.Y - height / 2.0f,
                    width, height);
            }
        }

        public void DrawLine(Graphics graphics, Color color, Point p1, Point p2)
        {
            using (var pen = new Pen(color))
            {
                graphics.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
            }
        }
        #endregion
        #endregion

        #region Private Methods
        private void Rescale()
        {
            MinX = _vertices.Min((point => Math.Min(MinX, point.X)));
            MinY = _vertices.Min((point => Math.Min(MinY, point.Y)));

            MaxX = _vertices.Max((point => Math.Max(MaxX, point.X)));
            MaxY = _vertices.Max((point => Math.Max(MaxY, point.Y)));

            Width = MaxX - MinX;
            Height = MaxY - MinY;

            Center = new Point((float)((MaxX + MinX) / 2.0f), (float)((MaxY + MinY) / 2.0f));

            CalculateSquare();
            CalculatePerimeter();
        }

        private Line LineAt(int index)
        {
            return new Line(_vertices[index], _vertices[IncrementIndex(index)]);
        }

        private Line GetLine(int index)
        {
            return LineAt(index);
        }

        private int DecrementIndex(int index)
        {
            return index == 0 ? _vertices.Count - 1 : index - 1;
        }

        private int IncrementIndex(int index)
        {
            return index >= _vertices.Count - 1 ? 0 : index + 1;
        }

        private Line ClosestLine(Point p)
        {
            return GetLine(IndexOfClosestLineTo(p));
        }

        private void CalculatePerimeter()
        {
            Perimeter = 0;
            var count = _vertices.Count;
            if (count < 2) return;

            for (var i = 0; i < count - 1; i++)
            {
                Perimeter += _vertices[i].DistanceTo(_vertices[i + 1]);
            }
            Perimeter += _vertices[count - 1].DistanceTo(_vertices[0]);
        }

        private void CalculateSquare()
        {
            Square = 0;
            var count = _vertices.Count;
            if (count < 3) return;

            for (var i = 0; i < count; i++)
            {
                var p0 = _vertices[i - 1 < 0 ? count - 1 : i - 1];
                var p1 = _vertices[i + 1 == count ? 0 : i + 1];
                Square += _vertices[i].X * (p1.Y - p0.Y);
            }
            Square = 0.5f * Math.Abs(Square);
        }

        private static void SwapIf<T>(ref T i, ref T j, Func<T, T, bool> func)
        {
            if (!func(i, j))
            {
                return;
            }
            var temp = j;
            j = i;
            i = temp;
        }

        private void SmoothRange(Point from, Point to, Func<int, List<Point>> function, int pointCount = 10)
        {
            if (from == null || to == null)
            {
                return;
            }

            var eps = 1.0f / pointCount;
            var start = IndexOfClosestPoint(from);
            var end = IndexOfClosestPoint(to);

            SwapIf(ref start, ref end, (i, i1) => i > i1);
            if (end - start < 0)
            {
                return;
            }

            var newPoints = new List<Point>();
            for (var i = start + 1; i < end - 1; i++)
            {
                var ab = function(i);
                for (var j = 1; j < pointCount; j++)
                {
                    var t = eps * j;
                    newPoints.Add(Point.Lerp(ab[0], ab[1], ab[2], ab[3], t));
                }
            }
            _vertices.RemoveRange(start + 1, end - start - 1);
            _vertices.InsertRange(start + 1, newPoints);
            Rescale();
        }

        private List<Point> BasicSpline(int i)
        {
            return new List<Point>
            {
                (_vertices[i - 1] + _vertices[i]*4 + _vertices[i + 1])/6,
                (-_vertices[i - 1] + _vertices[i + 1])/2,
                (_vertices[i - 1] - _vertices[i]*2 + _vertices[i + 1])/2,
                (-_vertices[i - 1] + _vertices[i]*3 - _vertices[i + 1]*3 + _vertices[i + 2])/6,
            };
        }

        private List<Point> InterSpline(int i)
        {
            return new List<Point>
            {
                _vertices[i],
                (-_vertices[i - 1] + _vertices[i + 1])/2,
                (_vertices[i - 1]*2 - _vertices[i]*5 + _vertices[i + 1]*4 - _vertices[i + 2])/2,
                (-_vertices[i - 1] + _vertices[i]*3 - _vertices[i + 1]*3 + _vertices[i + 2])/2,
            };
        }
        #endregion

        #region Object

        //public override string ToString()
        //{
        //    return
        //        $"{{\n\tRealVertices: {{ {Vertices.Aggregate(new StringBuilder(), (sb, p) => sb.Append(p).Append(", "))} }}, \n" +
        //        $"\tCenter: {Center}, \n" + $"\tWidth: {Width}, \n" + $"\tHeight: {Height}, \n" +
        //        $"\tSquare: {Square}, \n" + $"\tPerimeter: {Perimeter}\n}}";
        //}

        #endregion
    }
}

public class Amplifier
{
    public int Id { get; set; }
    public Point Delta { get; set; }

    public Amplifier(int id, Point delta)
    {
        Id = id;
        Delta = delta;
    }
}