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
        public void ExampleFuelCells()
        {
            var point = new Point(3, 5);
            var serial = 8;
            var cell = new FuelCell(point, serial);

            var point2 = new Point(122, 79);
            var serial2 = 57;
            var cell2 = new FuelCell(point2, serial2);

            var point3 = new Point(217, 196);
            var serial3 = 39;
            var cell3 = new FuelCell(point3, serial3);


            var point4 = new Point(101, 153);
            var serial4 = 71;
            var cell4 = new FuelCell(point4, serial4);
            Console.WriteLine($"{cell.PowerLevel} | {cell2.PowerLevel} | {cell3.PowerLevel} | {cell4.PowerLevel}");
            Assert.Multiple(() =>
            {
                Assert.AreEqual(4, cell.PowerLevel);
                Assert.AreEqual(-5, cell2.PowerLevel);
                Assert.AreEqual(0, cell3.PowerLevel);
                Assert.AreEqual(4, cell4.PowerLevel);
            });
        }
    }

}
