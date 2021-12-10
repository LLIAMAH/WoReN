using System;
using System.Collections.Generic;
using GeneralLib.Arrays;

namespace SortConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            const int m = 5;
            const int n = 5;
            var rnd = new Random();
            var array = ArraysEx.FillArrayByRandom(rnd, n, m);
            ArraysEx.WriteArray(array, n, m);

            var arrayMarked = new ArraysEx.MarkedOutput[n, m];
            for (var j = 0; j < m; j++)
            for (var i = 0; i < n; i++)
                arrayMarked[i, j] = new ArraysEx.MarkedOutput(array[i, j]);

            for (var j = 0; j < m; j++)
            {
                var arrayNewRow = SortingFunc(arrayMarked, j, n);
                for (var i = 0; i < n; i++)
                    arrayMarked[i, j] = arrayNewRow[i];
            }

            ArraysEx.WriteArray(arrayMarked, n, m);

            Console.WriteLine("Hello World!");
        }

        private static ArraysEx.MarkedOutput[] SortingFunc(ArraysEx.MarkedOutput[,] array, int rowIndex, int n)
        {
            var list = new List<ArraysEx.MarkedOutput>();
            for (var i = rowIndex + 1; i < n; i++)
                list.Add(array[i, rowIndex].SetMarked());

            list.Sort(delegate(ArraysEx.MarkedOutput x, ArraysEx.MarkedOutput y)
            {
                if (x == null && y == null) return 0;
                else if (x == null) return -1;
                else if (y == null) return 1;
                else return x.Value.CompareTo(y.Value);
            });

            var finalList = new List<ArraysEx.MarkedOutput>();

            var t = 0;
            while (t <= rowIndex)
            {
                finalList.Add(array[t, rowIndex]);
                t++;
            }

            finalList.AddRange(list);

            return finalList.ToArray();
        }

        
    }
}
