using NUnit.Framework;
using OutlineWF.Utilities;

namespace OutlineWF.Tests.Utilities
{
    [TestFixture]
    class LineTest
    {
        [Test]
        public void TestConstructor()
        {
            var p0 = new Point(0, 0);
            var p1 = new Point(1, 0);

            var line = new Line(new Point(0, 0), new Point(1, 0));

            Assert.AreEqual(line.P0, p0);
            Assert.AreEqual(line.P1, p1);
        }

        [Test]
        public void TestHorizontalCoefficients()
        {
            var p0 = new Point(0, 0);
            var p1 = new Point(1, 0);
            var line = new Line(new Point(0, 0), new Point(1, 0));
            Assert.AreEqual(line.A, 0);
            Assert.AreEqual(line.B, -1);
            Assert.AreEqual(line.C, 0);
            Assert.AreEqual(line.K, 0);
        }

        [Test]
        public void TestVerticalCoefficients()
        {
            var p0 = new Point(0, 0);
            var p1 = new Point(0, 1);
            var line = new Line(p0, p1);
            Assert.AreEqual(line.A, 1);
            Assert.AreEqual(line.B, 0);
            Assert.AreEqual(line.C, 0);
            Assert.AreEqual(line.K, float.NaN);
        }

        [Test]
        public void TestCoefficients()
        {
            var p0 = new Point(1, 1);
            var p1 = new Point(3, 2);
            var line = new Line(p0, p1);
            Assert.AreEqual(line.A, 1);
            Assert.AreEqual(line.B, -2);
            Assert.AreEqual(line.C, 1);
            Assert.AreEqual(line.K, 0.5);
        }

        [Test]
        public void TestHorizontalClosestPoint()
        {
            var p0 = new Point(0, 0);
            var p1 = new Point(2, 0);

            var line = new Line(p0, p1);

            var p = new Point(1, 1);

            var expected = new Point(1, 0);

            Assert.AreEqual(expected, line.ClosestPoint(p));
        }


        [Test]
        public void TestVerticalClosestPoint()
        {
            var p0 = new Point(0, 0);
            var p1 = new Point(0, 2);

            var line = new Line(p0, p1);

            var p = new Point(1, 1);

            var expected = new Point(0, 1);

            Assert.AreEqual(expected, line.ClosestPoint(p));
        }

        [Test]
        public void TestClosestPoint()
        {
            var p0 = new Point(5, 5);
            var p1 = new Point(65, 35);

            var line = new Line(p0, p1);

            var p = new Point(25, 40);

            var expected = new Point(35, 20);

            Assert.AreEqual(expected, line.ClosestPoint(p));
        }

        [Test]
        public void TestDistance()
        {
            var p0 = new Point(0, 0);
            var p1 = new Point(1, 0);
            var line = new Line(p0, p1);

            var point = new Point(9, 10);

            Assert.AreEqual(10, line.DistanceTo(point));
        }
    }
}
