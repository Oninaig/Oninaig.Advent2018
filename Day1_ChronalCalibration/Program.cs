﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1_ChronalCalibration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Read from file or enter values manually? (0,1)");
            var choice = Console.ReadLine().Trim();
            switch (choice)
            {
                case "0":
                    ReadFromInputFile();
                    break;
                case "1":
                    ManualInput();
                    break;
            }
           
            Console.ReadLine();
        }

        static void ReadFromInputFile()
        {
            var fileContents = File.ReadAllLines("inputs.txt");
            int frequency = 0;

            var frequencyDic = new Dictionary<int, bool>();
            frequencyDic[0] = true;

            var inputsDic = new Dictionary<string, bool>();

            foreach (var line in fileContents)
            {
                Console.WriteLine(line);
                bool shouldExit = false;

                //add the raw input to our inputs dictionary
                if(inputsDic.ContainsKey(line))
                    
                inputsDic[line] = true;
                
                var operation = line[0];
                int value = Convert.ToInt32(line.Substring(1, line.Length-1));
                var firstRepeat = -1;

                

                switch (operation)
                {
                    case '+':
                        frequency = frequency + value;
                        if (frequencyDic.ContainsKey(frequency))
                        {
                            firstRepeat = frequency;
                            shouldExit = true;
                        }
                        else
                            frequencyDic[frequency] = true;
                        break;
                    case '-':
                        frequency = frequency - value;
                        if (frequencyDic.ContainsKey(frequency))
                        {
                            firstRepeat = frequency;
                            shouldExit = true;
                        }
                        else
                            frequencyDic[frequency] = true;
                        break;
                }

                if (shouldExit)
                {
                    Console.WriteLine($"Found first duplicate frequency: {frequency}");
                    break;
                }
            }

            Console.WriteLine($"Final frequency: {frequency}");
        }

        static void ManualInput()
        {
            Console.WriteLine("Please enter the first input frequency.");
            int frequency = 0;
            var input = Console.ReadLine().Trim();
            var inputs = new List<string>();

            //use 'q' to quit or empty string
            while (!input.ToLower().Equals("q") && !input.Equals(string.Empty))
            {
                var operation = input[0];
                int value = Convert.ToInt32(input.Substring(1, input.Length-1));


                switch (operation)
                {
                    case '+':
                        frequency = frequency + value;
                        break;
                    case '-':
                        frequency = frequency - value;
                        break;
                }

                Console.Write("Finished operation. Enter next input, or 'q' to finish (empty string works too): ");

                inputs.Add(input);
                input = Console.ReadLine().Trim();
            }

            Console.WriteLine($"Your inputs were: {string.Join(",", inputs)}");
            Console.WriteLine($"Final frequency: {frequency}");
        }
    }
}
