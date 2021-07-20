using System;
using System.IO;

/*

https://adventofcode.com/2015/day/18

*/

namespace AdventOfCode
{
    public static class Program
    {
        const int GRID_HEIGHT_AND_WIDTH = 100;
        const int STEPS = 100;
        const int PART = 2;

        static void Main()
        {
            bool[,] currentState =
                new bool[GRID_HEIGHT_AND_WIDTH + 2, GRID_HEIGHT_AND_WIDTH + 2];

            bool[,] nextState =
                new bool[GRID_HEIGHT_AND_WIDTH + 2, GRID_HEIGHT_AND_WIDTH + 2];

            int[,] neighborsWithLightsOn =
                new int[GRID_HEIGHT_AND_WIDTH + 2, GRID_HEIGHT_AND_WIDTH + 2];

            ReadTestInput(currentState);

            Console.WriteLine("Initial state:");
            ShowState(currentState);

            for (var step = 1; step <= STEPS; step++)
            {
                int totalLightsOn = 0;

                for (int y = 1; y <= GRID_HEIGHT_AND_WIDTH; y++)
                {
                    for (int x = 1; x <= GRID_HEIGHT_AND_WIDTH; x++)
                    {
                        neighborsWithLightsOn[x, y] = 0;
                        for (int b = -1; b <= 1; b++)
                        {
                            for (int a = -1; a <= 1; a++)
                            {
                                if ((a == 0 && b == 0))
                                {
                                    continue;
                                }

                                if (currentState[x + a, y + b])
                                {
                                    neighborsWithLightsOn[x, y]++;
                                }
                            }
                            if (neighborsWithLightsOn[x, y] >= 4)
                            {
                                // There is no need to keep counting if we have reached 4, given the 
                                // rules for light changes at each step.
                                break;
                            }
                        }

                        nextState[x, y] =
                            PART == 1
                            ? GetPart1NextState(currentState[x, y], neighborsWithLightsOn[x, y])
                            : GetPart2NextState(x, y, currentState[x, y], neighborsWithLightsOn[x, y]);

                        totalLightsOn += Convert.ToInt32(nextState[x, y]);
                    }
                }

                Console.WriteLine("\nNeighbors with lights on:");
                ShowNeighborsWithLightsOn(neighborsWithLightsOn);

                currentState = nextState.Clone() as bool[,];

                Console.WriteLine(
                    $"\nAfter {step} step{(step > 1 ? "s" : "")} ({totalLightsOn} lights on):");
                ShowState(currentState);
            }
        }

        /// <summary>
        /// Returns true if the light at (x, y) is in a corner of the grid.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static bool IsCornerLight(int x, int y) =>
            (x == 1 && y == 1) ||
            (x == GRID_HEIGHT_AND_WIDTH && y == 1) ||
            (x == 1 && y == GRID_HEIGHT_AND_WIDTH) ||
            (x == GRID_HEIGHT_AND_WIDTH && y == GRID_HEIGHT_AND_WIDTH);

        private static void ReadTestInput(bool[,] state)
        {
            string input = File.ReadAllText("input.txt");

            using (StringReader reader = new StringReader(input))
            {
                string line = reader.ReadLine();

                for (int y = 1; y <= GRID_HEIGHT_AND_WIDTH; y++)
                {
                    for (int x = 1; x <= GRID_HEIGHT_AND_WIDTH; x++)
                    {
                        state[x, y] =
                            PART == 2 && IsCornerLight(x, y)
                            ? true
                            : line[x - 1] == '.'
                                ? false
                                : true;
                    }

                    line = reader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Returns true if a light whose current state is <c>currentState</c> should be turned on in
        /// the next step.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="neighborsWithLightsOn"></param>
        private static bool GetPart1NextState(bool currentState, int neighborsWithLightsOn)
        {
            bool nextState = currentState;

            if (currentState && !(neighborsWithLightsOn == 2 || neighborsWithLightsOn == 3))
            {
                return false;
            }
            else if (!currentState && neighborsWithLightsOn == 3)
            {
                return true;
            }

            return nextState;
        }

        /// <summary>
        /// Returns true if a light whose current state is <c>currentState</c> should be turned on in
        /// the next step.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="currentState"></param>
        /// <param name="neighborsWithLightsOn"></param>
        private static bool GetPart2NextState(
            int x, int y, bool currentState, int neighborsWithLightsOn)
        {
            bool nextState = currentState;

            if (IsCornerLight(x, y))
            {
                return true;
            }

            if (currentState && !(neighborsWithLightsOn == 2 || neighborsWithLightsOn == 3))
            {
                return false;
            }
            
            if (!currentState && neighborsWithLightsOn == 3)
            {
                return true;
            }

            return nextState;
        }

        /// <summary>
        /// Displays the light grid.
        /// </summary>
        /// <param name="state"></param>
        static void ShowState(bool[,] state)
        {
            for (int y = 1; y <= GRID_HEIGHT_AND_WIDTH; y++)
            {
                Console.Write("{0}\t", y);
                for (int x = 1; x <= GRID_HEIGHT_AND_WIDTH; x++)
                {
                    Console.Write(state[x, y] ? "#" : ".");
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Displays a grid showing how many of each light's neighbors are on.
        /// </summary>
        /// <param name="neighborsWithLightsOn"></param>
        static void ShowNeighborsWithLightsOn(int[,] neighborsWithLightsOn)
        {
            for (int y = 1; y <= GRID_HEIGHT_AND_WIDTH; y++)
            {
                Console.Write("{0}\t", y);
                for (int x = 1; x <= GRID_HEIGHT_AND_WIDTH; x++)
                {
                    Console.Write(neighborsWithLightsOn[x, y]);
                }

                Console.WriteLine();
            }
        }
    }
}
