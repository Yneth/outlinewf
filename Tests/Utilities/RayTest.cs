using System;
using NUnit.Framework;
using OutlineWF.Utilities;

namespace OutlineWF.Tests.Utilities
{
    [TestFixture]
    class RayTest
    {
        private Ray _r0;
        private Point _zero;

        [SetUp]
        public void Initialize()
        {
            _zero = new Point(0.0f, 0.0f);
            _r0 = new Ray(_zero, (float)(Math.Tan(45.0 * Math.PI / 180.0)));
        }

        [Test]
        public void TestGetPointFromStart()
        {
            Point p = _r0.GetPointFromStart(Math.Sqrt(2));
            Assert.AreEqual(1.0, p.X);
            Assert.AreEqual(1.0, p.Y);
        }
    }
}