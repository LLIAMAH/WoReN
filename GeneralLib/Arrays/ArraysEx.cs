using System;

namespace GeneralLib.Arrays
{
    public static class ArraysEx
    {
        public class MarkedOutput
        {
            private readonly int _value;
            private bool _marked;

            public int Value => _value;
            public bool Marked => _marked;

            public MarkedOutput(int data)
            {
                this._value = data;
                this._marked = false;
            }

            public MarkedOutput SetMarked()
            {
                this._marked = true;
                return this;
            }

            public override bool Equals(object? obj)
            {
                var converted = obj as MarkedOutput;
                if (converted == null)
                    return false;

                return this.Value == converted.Value;
            }
        }

        public static int[,] FillArrayByRandom(Random rnd, int sizeX, int sizeY)
        {
            var a = new int[sizeX, sizeY];
            for (var j = 0; j < sizeY; j++)
                for (var i = 0; i < sizeX; i++)
                    a[i, j] = rnd.Next(0, +100);

            return a;
        }

        public static void WriteArray(int[,] array, int cols, int rows)
        {
            for (var j = 0; j < rows; j++)
            {
                for (var i = 0; i < cols; i++)
                    Console.Write($"{array[i,j]}\t");

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static void WriteArray(MarkedOutput[,] array, int cols, int rows)
        {
            for (var j = 0; j < rows; j++)
            {
                for (var i = 0; i < cols; i++)
                    if (array[i, j].Marked)
                    {
                        var backgroundColor = Console.BackgroundColor;
                        var foreColor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{array[i, j].Value}\t");
                        Console.ForegroundColor = foreColor;
                        Console.BackgroundColor = backgroundColor;
                    }
                    else
                    {
                        Console.Write($"{array[i, j].Value}\t");
                    }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
