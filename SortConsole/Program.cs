﻿using System;
using System.Collections.Generic;

namespace SortConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int m = 10;
            const int n = 20;
            var array = AssignData(m, n);

            WriteArray(array, n,m);

            for (var j = 0; j < m; j++)
            {
                var arrayNewRow = SortingFunc(array, j, n);
                for (var i = 0; i < n; i++)
                    array[i, j] = arrayNewRow[i];
            }

            WriteArray(array, n, m);

            Console.WriteLine("Hello World!");
        }

        private static int[] SortingFunc(int[,] array, int rowIndex, int n)
        {
            var list = new List<int>();
            for (var i = rowIndex; i < n; i++)
            {
                list.Add(array[i, rowIndex]);
            }

            list.Sort();

            var t = 0;
            while (t < rowIndex)
            {
                list.Insert(0, array[t, rowIndex]);
                t++;
            }

            return list.ToArray();
        }

        private static void WriteArray(int[,] array, int columns, int rows)
        {
            throw new NotImplementedException();
        }

        private static int[,] AssignData(int i, int i1)
        {
            throw new NotImplementedException();
        }
    }
}
