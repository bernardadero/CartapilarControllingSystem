using System;
using System.Collections.Generic;
using System.IO;


namespace CartapilarControllingSystem
{
    class Program
    {
        static void Main()
        {
            const int planetSize = 30;  // Size of the planet
            Caterpillar caterpillar = new Caterpillar(planetSize);

            while (true)
            {
                caterpillar.DisplayRadarImage();
                Console.WriteLine("Enter command (Direction and distance for example up one = U 1/down two = D 2/left three = L 3/right one R 1/grow = G/shrink =S/Undo/Redo/Exit ): ");

                string input = Console.ReadLine()?.Trim().ToUpper();

                char direction = input[0];
                int steps = 1;
                try
                {
                    //check if steps given is greater than one
                    if (int.Parse(input.Substring(2)) >= 1)
                    {
                        steps = int.Parse(input.Substring(2));
                    }
                }
                catch { }
                switch (direction.ToString())
                {
                    case "U":
                        caterpillar.Move(Direction.Up, steps);
                        break;
                    case "D":
                        caterpillar.Move(Direction.Down, steps);
                        break;
                    case "L":
                        caterpillar.Move(Direction.Left, steps);
                        break;
                    case "R":
                        caterpillar.Move(Direction.Right, steps);
                        break;
                    case "G":
                        caterpillar.Grow();
                        break;
                    case "S":
                        caterpillar.Shrink();
                        break;
                    case "UNDO":
                        caterpillar.Undo();
                        break;
                    case "REDO":
                        caterpillar.Redo();
                        break;
                    case "EXIT":
                        return;
                    default:
                        Console.WriteLine("Invalid command. Try again.");
                        break;
                }
            }
        }
    }
}