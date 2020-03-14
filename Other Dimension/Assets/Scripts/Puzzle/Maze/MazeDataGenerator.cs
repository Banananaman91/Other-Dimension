using UnityEngine;

namespace Puzzle.Maze
{
    public class MazeDataGenerator
    {
        private float _placementThreshold;

        public MazeDataGenerator()
        {
            _placementThreshold = 0.1f;
        }

        public int[,] FromDimensions(int sizeRows, int sizeCols)
        {
            int[,] maze = new int[sizeRows, sizeCols];
            int rMax = maze.GetUpperBound(0);
            int cMax = maze.GetUpperBound(1);
            for (int i = 0; i <= rMax; i++)
            {
                for (int j = 0; j <= cMax; j++)
                {
                    if (i == 0 || j == 0 || i == rMax || j == cMax) maze[i, j] = 0; // places walls around the outside
                    else if (i % 2 == 0 && j % 2 == 0) // checks if the coordinate is divisible by 2 to get every other cell
                    {
                        if (Random.value > _placementThreshold)
                        {
                            // assign 1 to the current cell and a randomly chose adjacent cell
                            maze[i, j] = 1;
                            int a = Random.value > 0.5 ? 0 : (Random.value < 0.5 ? -1 : 1);
                            int b = a != 0 ? 0 : (Random.value < 0.5 ? -1 : 1);
                            maze[i + a, j + b] = 1;
                        }
                    }
                }
            }

            return maze;
        }
    }
}
