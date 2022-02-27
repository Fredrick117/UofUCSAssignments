/* Carl Fredrick - solution to Grid Gobble Kattis problem for CS 4150 written in C#
 * https://utah.kattis.com/problems/utah.gobble
 * Source: Kattis
 * License: CC BY-SA 3.0
 * 
 * Creative Commons license information - https://creativecommons.org/licenses/by-sa/3.0/#
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace GridGobble
{
	class Program
	{
		public static int[,] grid;
		public static int[,] cache;
		public static int M, N;

		static void Main(string[] args)
		{
			// Get the first line of input, which contains the number of rows and columns
			string[] firstLine = Console.ReadLine().Split(' ');

			// Grid dimensions
			M = Int32.Parse(firstLine[0]);
			M++;
			N = Int32.Parse(firstLine[1]);

			grid = new int[M, N];
			cache = new int[M, N];

			// Populate the grid
			for (int i = 0; i < M - 1; i++)
			{
				string[] rowNums = Console.ReadLine().Split(' ');
				for (int j = 0; j < N; j++)
				{
					grid[i, j] = Int32.Parse(rowNums[j]);
				}
			}

			// Populate cache with smallest possible integer values
			for (int i = 0; i < M; i++)
			{
				for (int j = 0; j < N; j++)
				{
					cache[i, j] = int.MinValue;
				}
			}

			Console.WriteLine(GridGobble());
		}

		// Driver function
		public static int GridGobble()
		{
			// Retrieve a list of all possible solutions
			List<int> solutions = new List<int>();
			for (int i = 0; i <= N - 1; i++)
				solutions.Add(GridGobble(M - 1, i));

			return solutions.Max();
		}

		/*
		 * Three possible moves:
		 * 1) Directly above (i-1, j)
		 * 2) Diagonally above to the left (i-1, j-1)
		 * 3) Diagonally above to the right (i-1, j+1)
		 */
		public static int GridGobble(int r, int c)
		{
			// Base case
			if (r == 0) {
				cache[r, c] = 0;
				return 0;
			}

			if (cache[r, c] != int.MinValue)
				return cache[r, c];

			/*
			 * If the current(r, c) pair is in the first column, then a diagonal left move wraps around to the last column(direction = 0)
			 * If the current(r, c) pair is in the last column, then a diagonal right move wraps around to the first column(direction = 1)
			 * Otherwise, move normally(direction = 2)
			 */
			int up = grid[r - 1, c];
			int left, right;

			if (c == 0)
			{
				left = grid[r - 1, c + (N - 1)];
				right = grid[r - 1, c + 1];

				cache[r, c] = Max(
					GridGobble(r - 1, c) + up,
					GridGobble(r - 1, c + (N - 1)) - left,
					GridGobble(r - 1, c + 1) - right
				);
			}
			else if (c == (N - 1))
			{
				left = grid[r - 1, c - 1];
				right = grid[r - 1, c - (N - 1)];

				cache[r, c] = Max(
					GridGobble(r - 1, c) + up,
					GridGobble(r - 1, c - 1) - left,
					GridGobble(r - 1, c - (N-1)) - right
				);
			}
			else
			{
				left = grid[r - 1, c - 1];
				right = grid[r - 1, c + 1];

				cache[r, c] = Max(
					GridGobble(r - 1, c) + up,
					GridGobble(r - 1, c - 1) - left,
					GridGobble(r - 1, c + 1) - right
				);
			}

			return cache[r, c];
		}

		// Finds the largest of three integers
		public static int Max(int a, int b, int c) {
			return Math.Max(a, Math.Max(b, c));
		}
	}
}
