using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Day11_ChronalCharge
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ExampleFuelCell()
        {
            var point = new Point(3, 5);
            var serial = 8;
            var cell = new FuelCell(point, serial);
            Assert.AreEqual(4, cell.PowerLevel);
        }
    }

}
