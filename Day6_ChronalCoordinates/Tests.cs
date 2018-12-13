using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace Day6_ChronalCoordinates
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ManhattanDistanceTwoPoints()
        {
            
            var p1 = new Coordinate(5, 4);
            var p2 = new Coordinate(4, 6);

            var distance = p1.ManhanttanDistance(p2);
            Assert.AreEqual(3, distance);

        }

        [Test]
        public void ManhattanDistanceTotal()
        {
            var points = new List<Coordinate>()
            {
                new Coordinate(-1, 5),
                new Coordinate(1, 6),
                new Coordinate(3, 5),
                new Coordinate(2, 3)

            };

            var totalDistance = 0;
            var firstDistance = 0;
            var secondDistance = 0;

            var thirdDistance = 0;

            for (int i = 0; i < points.Count; i++)
            {
                var currPoint = points[i];
                for (int j = i+1; j < points.Count; j++)
                {
                    var nextPoint = points[j];
                    var distance = currPoint.ManhanttanDistance(nextPoint);
                    totalDistance += distance;

                    switch (i)
                    {
                        case 0:
                            firstDistance += distance;
                            break;
                        case 1:
                            secondDistance += distance;
                            break;
                        case 2:
                            thirdDistance += distance;
                            break;
                    }
                }
            }

            var firstAndSecond = firstDistance + secondDistance;
            var firstAndSecondAndThird = firstAndSecond + thirdDistance;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(firstDistance, 12);
                Assert.AreEqual(secondDistance, 7);
                Assert.AreEqual(thirdDistance, 3);
                Assert.AreEqual(firstAndSecond, 19);
                Assert.AreEqual(totalDistance, firstAndSecondAndThird);
                Assert.AreEqual(totalDistance, 22);
            });
        }

        [Test]
        public void TestCanReadFile()
        {
            var input = File.ReadAllLines(TestContext.CurrentContext.TestDirectory + "\\testinput.txt");
            Console.WriteLine(input.Length);
            Assert.Greater(input.Length, 0);
        }

        [Test]
        public void HighetLowestPoint()
        {
            var input = File.ReadAllLines(TestContext.CurrentContext.TestDirectory + "\\testinput.txt");
            Console.WriteLine($"Lines found: {input.Length}");
            
            var manager= new ChronalCoordinateManager();
            foreach (var line in input)
                manager.AddCoordinate(line.Split(',')[0], line.Split(',')[1]);

            manager.InitMasterGrid();

            var topRight = new Coordinate(352, 2000);
            var bottomLeft = new Coordinate(-100, -423);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(topRight, manager.GridMeta.TopRight);
                Assert.AreEqual(bottomLeft, manager.GridMeta.BottomLeft);
            });
        }
    }
}
