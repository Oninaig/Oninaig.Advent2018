using System;
using System.Collections.Generic;
using System.IO;

namespace Day1_ChronalCalibration
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Read from file or enter values manually? (0,1)");
            var choice = Console.ReadLine().Trim();
            switch (choice)
            {
                case "0":
                    Console.Write(
                        "Do you want to process the data normally or look for repeating frequencies? (0,1): ");
                    var repeatChoice = Convert.ToBoolean(Convert.ToInt16(Console.ReadLine().Trim()));
                    ReadFromInputFile(repeatChoice);
                    break;
                case "1":
                    ManualInput();
                    break;
            }

            Console.ReadLine();
        }

        private static void ProcessData(string[] data, int frequency)
        {
            foreach (var line in data)
            {
                Console.WriteLine(line);
                var operation = line[0];
                var value = Convert.ToInt32(line.Substring(1, line.Length - 1));

                switch (operation)
                {
                    case '+':
                        frequency = frequency + value;
                        break;
                    case '-':
                        frequency = frequency - value;
                        break;
                }
            }

            Console.WriteLine($"Final frequency: {frequency}");
        }

        private static void ProcessDataForRepeats(string[] data, int frequency)
        {
            // Frequency dictionary to keep track of frequencies we've seen.
            var frequencyDic = new Dictionary<int, bool>();
            frequencyDic[0] = true;

            var foundRepeat = false;
            while (foundRepeat == false)
                foreach (var line in data)
                {
                    Console.WriteLine(line);
                    var shouldExit = false;
                    var operation = line[0];
                    var value = Convert.ToInt32(line.Substring(1, line.Length - 1));
                    switch (operation)
                    {
                        case '+':
                            frequency = frequency + value;
                            if (frequencyDic.ContainsKey(frequency))
                                shouldExit = true;
                            else
                                frequencyDic[frequency] = true;
                            break;
                        case '-':
                            frequency = frequency - value;
                            if (frequencyDic.ContainsKey(frequency))
                                shouldExit = true;
                            else
                                frequencyDic[frequency] = true;
                            break;
                    }

                    if (shouldExit)
                    {
                        Console.WriteLine($"Found first duplicate frequency: {frequency}");
                        foundRepeat = true;
                        break;
                    }
                }
        }


        private static void ReadFromInputFile(bool findRepeatingFrequency)
        {
            var fileContents = File.ReadAllLines("inputs.txt");
            var frequency = 0;

            switch (findRepeatingFrequency)
            {
                case true:
                    ProcessDataForRepeats(fileContents, frequency);
                    break;
                case false:
                    ProcessData(fileContents, frequency);
                    break;
            }
        }

        private static void ManualInput()
        {
            Console.WriteLine("Please enter the first input frequency.");
            var frequency = 0;
            var input = Console.ReadLine().Trim();
            var inputs = new List<string>();

            //use 'q' to quit or empty string
            while (!input.ToLower().Equals("q") && !input.Equals(string.Empty))
            {
                var operation = input[0];
                var value = Convert.ToInt32(input.Substring(1, input.Length - 1));


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