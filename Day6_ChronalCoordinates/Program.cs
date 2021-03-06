﻿using System;
using System.IO;

namespace Day6_ChronalCoordinates
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Sample grid
            //var sampleManager = new ChronalCoordinateManager();
            //sampleManager.AddCoordinate(1,1);
            //sampleManager.AddCoordinate(1,6);
            //sampleManager.AddCoordinate(8,3);
            //sampleManager.AddCoordinate(3,4);
            //sampleManager.AddCoordinate(5,5);
            //sampleManager.AddCoordinate(8,9);
            //sampleManager.InitMasterGrid();
            //sampleManager.PrintGrid();
            //sampleManager.FindAreas();

            //Console.ReadLine();

            var input = File.ReadAllLines("puzzleinput.txt");

            var manager = new ChronalCoordinateManager();
            foreach (var line in input)
                manager.AddCoordinate(line.Split(',')[0], line.Split(',')[1]);

            manager.InitMasterGrid();
            manager.FindAreas();
            manager.FindSafeRegion();
            Console.ReadLine();
        }
    }
}