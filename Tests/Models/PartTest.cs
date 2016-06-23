using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OutlineWF.Models;
using OutlineWF.Utilities;

namespace OutlineWF.Tests.Models
{
    [TestFixture]
    class PartTest
    {
        private Part _part;
        private Point _scale;
        private List<Point> _points;

        [SetUp]
        public void Initialize()
        {
            _scale = new Point(1, 1);

            _part = new Part();

            _points = new List<Point>
            {
                new Point(0, 0),
                new Point(0, 10),
                new Point(10, 10),
                new Point(10, 0)
            };

            _part.AddPoint(_points[0] * _scale);
            _part.AddPoint(_points[1] * _scale);
            _part.AddPoint(_points[2] * _scale);
            _part.AddPoint(_points[3] * _scale);
        }

        [Test]
        public void TestSquare()
        {
            Assert.AreEqual(100, _part.Square);
        }

        [Test]
        public void TestCenter()
        {
            Assert.AreEqual(new Point(5, 5), _part.Center);
        }

        [Test]
        public void TestPerimeter()
        {
            Assert.AreEqual(40, _part.Perimeter);
        }

        [Test]
        public void TestMinValues()
        {
            Assert.AreEqual(0, _part.MinX);
            Assert.AreEqual(0, _part.MinY);
        }

        [Test]
        public void TestMaxValues()
        {
            Assert.AreEqual(10, _part.MaxX);
            Assert.AreEqual(10, _part.MaxY);
        }

        [Test]
        public void TestWidth()
        {
            Assert.AreEqual(10, _part.Width);
        }

        [Test]
        public void TestHeight()
        {
            Assert.AreEqual(10, _part.Height);
        }

        [Test]
        public void TestCompressOnEmptyPart()
        {
            var p = new Part();
            p.Compress();

            Assert.AreEqual(0, p.Vertices.Count);
        }

        [Test]
        public void TestCompressOnInitialPart()
        {
            _part.Compress();

            Assert.AreEqual(_points.Count, _part.Vertices.Count);
        }

        [Test]
        public void TestAdd()
        {
            var p = new Point(0, 1);
            _part.AddPoint(p, 10, 10);

            Assert.AreEqual(p, _part.Vertices[1]);
        }

        [Test]
        public void TestFindReal()
        {
            var p = new Point(0, 1);
            _part.AddPoint(p, 10, 10);

            Assert.AreEqual(p, _part.ClosestLocalPointTo(p, 10, 10));
        }

        [Test]
        public void TestFindRealVeryClose()
        {
            var p = new Point(0, 1);
            var p1 = new Point(0, 0.5f);
            _part.AddPoint(p, 10, 10);
            _part.AddPoint(p1, 10, 10);

            Assert.AreEqual(p, _part.ClosestLocalPointTo(p, 10, 10));
        }

        [Test]
        public void TestCompress()
        {
            _part.AddPoint(new Point(0, 1), 10, 10);
            _part.AddPoint(new Point(0, 2), 10, 10);
            _part.AddPoint(new Point(0, 3), 10, 10);
            _part.AddPoint(new Point(0, 4), 10, 10);
            _part.AddPoint(new Point(0, 5), 10, 10);
            _part.AddPoint(new Point(0, 6), 10, 10);
            _part.AddPoint(new Point(0, 7), 10, 10);
            _part.AddPoint(new Point(0, 8), 10, 10);
            _part.AddPoint(new Point(0, 9), 10, 10);

            _part.Compress();

            Assert.AreEqual(_points.Count, _part.Vertices.Count);
        }

        [Test]
        public void TestCompressTwice()
        {
            _part.AddPoint(new Point(0, 1), 10, 10);
            _part.AddPoint(new Point(0, 2), 10, 10);
            _part.AddPoint(new Point(0, 3), 10, 10);
            _part.AddPoint(new Point(0, 4), 10, 10);
            _part.AddPoint(new Point(0, 5), 10, 10);
            _part.AddPoint(new Point(0, 6), 10, 10);
            _part.AddPoint(new Point(0, 7), 10, 10);
            _part.AddPoint(new Point(0, 8), 10, 10);
            _part.AddPoint(new Point(0, 9), 10, 10); ;

            _part.Compress();
            _part.Compress();

            Assert.AreEqual(_points.Count, _part.Vertices.Count);
        }

        [Test]
        public void TestBSplineSmoothRange()
        {
            _part.BSplineSmoothRange(_points[0], _points[3]);

            Assert.AreEqual(11, _part.Vertices.Count);
            Assert.AreEqual(_points[0], _part.Vertices[0]);
            Assert.AreEqual(_points[3], _part.Vertices[_part.Vertices.Count - 1]);
        }

        [Test]
        public void TestISplineSmoothRange()
        {
            _part.InterSplineSmoothRange(_points[0], _points[3]);

            Assert.AreEqual(11, _part.Vertices.Count);
            Assert.IsTrue(_part.Vertices.Contains(_points[0]));
            Assert.IsTrue(_part.Vertices.Contains(_points[1]));
            Assert.IsTrue(_part.Vertices.Contains(_points[2]));
            Assert.IsTrue(_part.Vertices.Contains(_points[3]));
        }

        [Test]
        public void TestISplineSmoothRangeWithNullParameters()
        {
            _part.InterSplineSmoothRange(_points[3], null);
            _part.InterSplineSmoothRange(null, null);
            _part.InterSplineSmoothRange(null, _points[0]);

            Assert.AreEqual(4, _part.Vertices.Count);
            Assert.IsTrue(_part.Vertices.Contains(_points[0]));
            Assert.IsTrue(_part.Vertices.Contains(_points[1]));
            Assert.IsTrue(_part.Vertices.Contains(_points[2]));
            Assert.IsTrue(_part.Vertices.Contains(_points[3]));
        }

        [Test]
        public void TestBSplineSmoothRangeWithNullParameters()
        {
            _part.BSplineSmoothRange(_points[3], null);
            _part.BSplineSmoothRange(null, null);
            _part.BSplineSmoothRange(null, _points[0]);

            Assert.AreEqual(4, _part.Vertices.Count);
            Assert.IsTrue(_part.Vertices.Contains(_points[0]));
            Assert.IsTrue(_part.Vertices.Contains(_points[1]));
            Assert.IsTrue(_part.Vertices.Contains(_points[2]));
            Assert.IsTrue(_part.Vertices.Contains(_points[3]));
        }
    }
}
