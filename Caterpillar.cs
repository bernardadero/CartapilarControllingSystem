using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CartapilarControllingSystem
{
    // Enum to represent directions
    public enum Direction { Up, Down, Left, Right }

    // Enum to represent the caterpillar segments
    public enum CaterpillarSegment { Head, Tail, Intermediate }

    // Enum to represent objects on the planet
    public enum PlanetObject { Empty, Spice, Booster, Obstacle }
    public class Caterpillar
    {
        private List<(int x, int y)> segments;  // List of (x, y) coordinates representing segments
        private int maxLength = 5;  // Maximum length of the caterpillar
        private List<string> commandLog = new List<string>();
        private PlanetObject[,] planet;

        public Caterpillar(int planetSize)
        {
            segments = new List<(int x, int y)> { (0, 0), (0, 1) };
            planet = new PlanetObject[planetSize, planetSize];
            InitializePlanet(planetSize);
        }

        public IReadOnlyList<(int x, int y)> Segments => segments;

        public PlanetObject[,] Planet => planet;

        // Initialize the planet with obstacles, spice, and boosters
        private void InitializePlanet(int planetSize)
        {
            // Logic to initialize the planet
            // For simplicity, let's place an obstacle at (10, 10), spice at (15, 15), and a booster at (20, 20)
            planet[10, 10] = PlanetObject.Obstacle;
            planet[15, 15] = PlanetObject.Spice;
            planet[20, 20] = PlanetObject.Booster;
        }

        // Move the caterpillar in the specified direction for the given number of steps
        public void Move(Direction direction, int steps)
        {
            // Logic to move the caterpillar
            for (int i = 0; i < steps; i++)
            {
                // Implement logic to check for obstacles, spice, boosters, and update caterpillar position
                (int newX, int newY) = GetNewPosition(direction);
                segments.Insert(0, (newX, newY));  // Insert the new head
                segments.RemoveAt(segments.Count - 1);  // Remove the tail
                if( IsObstacle(newX, newY))
                 {
                    return;
                }
                // Check if the tail needs to be adjusted
                AdjustTail();
                


            }
            // Log the command for later analysis
            LogCommand($"Move {direction} {steps} steps");
        }



        // Check if the specified position contains an obstacle
        public bool IsObstacle(int x, int y)
        {
            return x < 0 || x >= 30 || y < 0 || y >= 30 || planet[x, y] == PlanetObject.Obstacle;
        }


        // Grow the caterpillar by adding a new segment at the end
        public void Grow()
        {
            // Logic to grow the caterpillar
            if (segments.Count < maxLength)
            {
                (int tailX, int tailY) = segments[segments.Count - 1];
                segments.Add((tailX, tailY));
            }

            // Log the command for later analysis
            LogCommand("Grow");
        }

        // Shrink the caterpillar by removing the last segment
        public void Shrink()
        {
            // Logic to shrink the caterpillar
            if (segments.Count > 2)  // Ensure there are at least two segments (head and tail)
            {
                segments.RemoveAt(segments.Count - 1);
            }

            // Log the command for later analysis
            LogCommand("Shrink");
        }

        // Adjust the tail position if needed
        private void AdjustTail()
        {
            // Check if the head and tail aren't touching and aren't in the same row or column
            if (!AreAdjacent(segments[0], segments[segments.Count - 1]) &&
                !AreInSameRowOrColumn(segments[0], segments[segments.Count - 1]))
            {
                // Move the tail one step diagonally
                (int tailX, int tailY) = segments[segments.Count - 1];
                (int newTailX, int newTailY) = GetNewPosition(GetDirectionToHead(tailX, tailY));
                segments[segments.Count - 1] = (newTailX, newTailY);
            }
        }

       
        // Check if two positions are adjacent
        private bool AreAdjacent((int x, int y) position1, (int x, int y) position2)
        {
            int dx = Math.Abs(position1.x - position2.x);
            int dy = Math.Abs(position1.y - position2.y);
            return dx <= 1 && dy <= 1;
        }


        // Get the direction from tail to head
        private Direction GetDirectionToHead(int tailX, int tailY)
        {
            (int headX, int headY) = segments[0];

            if (tailX < headX)
            {
                return Direction.Right;
            }
            else if (tailX > headX)
            {
                return Direction.Left;
            }
            else if (tailY < headY)
            {
                return Direction.Down;
            }
            else
            {
                return Direction.Up;
            }
        }

        // Check if two positions are in the same row or column
        private bool AreInSameRowOrColumn((int x, int y) position1, (int x, int y) position2)
        {
            return position1.x == position2.x || position1.y == position2.y;
        }

        // Log commands to a file
        private void LogCommand(string command)
        {
            // for testing purpose
            string logFilePath = @"C:\Logs\MyLog.txt";
            // Create a logger instance with the specified file path
            Logger logger = new Logger(logFilePath);

            // Example: Log a message
            logger.Log("Command Issued: " + command);

            commandLog.Add(command);
           

        }

        // Undo the last command
        public void Undo()
        {
            if (commandLog.Count > 0)
            {
                string lastCommand = commandLog[commandLog.Count - 1];
                ExecuteUndoRedo(lastCommand);
                commandLog.RemoveAt(commandLog.Count - 1);
            }
        }

        // Redo the last undone command
        public void Redo()
        {
            // Implement redo logic if needed
            // ...
        }

        // Execute undo or redo based on the command
        private void ExecuteUndoRedo(string command)
        {
            // Logic to execute undo or redo based on the command
            // ...
        }

        // Display the radar image
        public void DisplayRadarImage()
        {
            // Logic to display the radar image
            // For simplicity, let's print the planet to the console
            for (int y = 0; y < planet.GetLength(0); y++)
            {
                for (int x = 0; x < planet.GetLength(1); x++)
                {
                    if (segments.Contains((x, y)))
                    {
                        Console.Write("H");  // Head or segment
                    }
                    else if (segments.Contains((x, y + 1)))
                    {
                        Console.Write("T");
                    }
                    else if (planet[x, y] == PlanetObject.Spice)
                    {
                        Console.Write("$");  // Spice
                    }
                    else if (planet[x, y] == PlanetObject.Booster)
                    {
                        Console.Write("B");  // Booster
                    }
                    else if (planet[x, y] == PlanetObject.Obstacle)
                    {
                        Console.Write("#");  // Obstacle
                    }
                    else
                    {
                        Console.Write(".");  // Print dot for empty space
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        // Get the new position based on the current direction
        private (int x, int y) GetNewPosition(Direction direction)
        {
            (int headX, int headY) = segments[0];
            return direction switch
            {
                Direction.Up => (headX, headY - 1),
                Direction.Down => (headX, headY + 1),
                Direction.Left => (headX - 1, headY),
                Direction.Right => (headX + 1, headY),
                _ => (headX, headY),
            };
        }
    }
}
