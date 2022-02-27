/* Carl Fredrick - solution to Narrow Art Gallery Kattis problem for CS 4150 written in C#
 * https://utah.kattis.com/problems/narrowartgallery
 * 
 * Author of Narrow Art Gallery problem: Robert Hochberg
 * Source: 2014 ICPC North America Qualifier
 * License: CC BY 3.0
 * 
 * Creative Commons license information - https://creativecommons.org/licenses/by/3.0/
 */

using System;
using System.Collections.Generic;

namespace NarrowArtGallery
{
    class Program
    {
        public static int N;
        public static int[,] values;
        public static int[,,] cache;

        static void Main(string[] args)
        {
            string[] s = Console.ReadLine().Split(' ');

            // Number of total rows
            N = Int32.Parse(s[0]);

            // Number of rooms that must be closed off
            int k = Int32.Parse(s[1]);

            values = new int[N, 2];
            cache = new int[N, 3, k + 1];

            // Populate the cache with '3' values as placeholders so that they can be looked up and populated with proper values when needed.
            for (int x = 0; x < N; x++)
                for (int y = 0; y < 3; y++)
                    for (int z = 0; z <= k; z++)
                        cache[x, y, z] = 3;
                        

            for (int i = 0; i < N; i++) {
                string[] integers = Console.ReadLine().Split(' ');
                values[i, 0] = Int32.Parse(integers[0]);
                values[i, 1] = Int32.Parse(integers[1]);
            }

            Console.WriteLine(maxValue(0, 2, k));
        }

        /*
        * Uncloseable room is either:
        * 0 = the room in column 0 of row r cannot be closed
        * 1 = the room in column 1 of row r cannot be closed
        * 2 = either room (but not both) of row r may be closed if desired
        */
        private static int maxValue(int r, int uncloseableRoom, int k) {
            if (r == N || k < 0)
                return 0;

            // A door MUST be closed
            if (k == N - r) {
                if (uncloseableRoom == 0 || uncloseableRoom == 1)
                {
                    if (cache[r, uncloseableRoom, k] == 3)
                        cache[r, uncloseableRoom, k] = values[r, uncloseableRoom] + maxValue(r + 1, uncloseableRoom, k - 1);
                    else
                        return cache[r, uncloseableRoom, k];
                }
                else if (uncloseableRoom == 2)
                {
                    if (cache[r, 2, k] == 3)
                    {
                        cache[r, 2, k] = Math.Max(
                            values[r, 0] + maxValue(r + 1, 0, k - 1),
                            values[r, 1] + maxValue(r + 1, 1, k - 1)
                        );
                    }
                    else
                    {
                        return cache[r, 2, k];
                    }
                }
            }
            // A door doesn't HAVE to be closed, but can be if desired
            else if (k < (N - r))
            {
                if (uncloseableRoom == 0 || uncloseableRoom == 1)
                {
                    if (cache[r, uncloseableRoom, k] == 3)
                    {
                        cache[r, uncloseableRoom, k] = Math.Max(
                            values[r, uncloseableRoom] + maxValue(r + 1, uncloseableRoom, k - 1),
                            values[r, 0] + values[r, 1] + maxValue(r + 1, 2, k)
                        );
                    }
                    else 
                    {
                        return cache[r, uncloseableRoom, k];
                    }
                }
                else if (uncloseableRoom == 2)
                {
                    if (cache[r, 2, k] == 3)
                    {
                        cache[r, 2, k] = Math.Max(
                            values[r, 0] + maxValue(r + 1, 0, k - 1),
                            Math.Max(
                                values[r, 1] + maxValue(r + 1, 1, k - 1),
                                values[r, 0] + values[r, 1] + maxValue(r + 1, 2, k)
                            )
                        );
                    }
                    else
                        return cache[r, 2, k];
                }
            }
            return cache[r, uncloseableRoom, k];
        }
    }
}
