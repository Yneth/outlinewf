using System;
using NUnit.Framework;
using OutlineWF.Utilities;

namespace OutlineWF.Tests.Utilities
{
    [TestFixture]
    class PointTest
    {
        private Point _p0;
        private Point _p1;
        private Point _p2;
        private Point _p3;

        [SetUp]
        public void Initialize()
        {
            _p0 = new Point(0, 0);
            _p1 = new Point(0, 10);
            _p2 = new Point(10, 10);
            _p3 = new Point(10, 0);
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreEqual(0, _p0.X);
            Assert.AreEqual(0, _p0.Y);
        }

        [Test]
        public void TestDistanceTo()
        {
            Assert.AreEqual(10, _p0.DistanceTo(_p1));
            Assert.AreEqual((float)(10 * Math.Sqrt(2)), _p0.DistanceTo(_p2));
        }

        [Test]
        public void TestSquaredDistanceTo()
        {
            Assert.AreEqual(100, _p0.SquareDistanceTo(_p1));
            Assert.AreEqual(200, _p0.SquareDistanceTo(_p2));
        }

        [Test]
        public void TestNegate()
        {
            Assert.AreEqual(new Point(-_p2.X, -_p2.Y), -_p2);
        }

        [Test]
        public void TestSum()
        {
            Assert.AreEqual(new Point(20, 20), _p2 + _p2);
            Assert.AreEqual(new Point(20, 20), _p2 + 10);
        }

        [Test]
        public void TestDiff()
        {
            Assert.AreEqual(new Point(0, 0), _p2 - _p2);
            Assert.AreEqual(new Point(0, 0), _p2 - 10);
        }

        [Test]
        public void TestDivide()
        {
            Assert.AreEqual(new Point(1, 1), _p2 / _p2);
            Assert.AreEqual(new Point(1, 1), _p2 / 10);
        }

        [Test]
        public void TestMultiply()
        {
            Assert.AreEqual(new Point(100, 100), _p2 * _p2);
            Assert.AreEqual(new Point(100, 100), _p2 * 10);
        }
    }
}
