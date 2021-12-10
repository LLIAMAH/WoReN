using System;
using System.Collections.Generic;
using GeneralLib.Arrays;

namespace ConsoleApp
{
    internal class Program
    {
        private static Random _rnd = new Random();
        private enum Direction
        {
            Current,
            ShiftRight,
            GoLeft,
            ShiftLeft,
            GoRight
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter Matrix minor size: ");
            var inputSize = Console.ReadLine();

            var n = 0;
            try
            {
                n = Convert.ToInt32(inputSize);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Restart app and enter the number for Matrix minor size.");
                return;
            }
            
            var m = 2 * n;
            var array = ArraysEx.FillArrayByRandom(_rnd, n, m);
            DrawArray(array, n, m);
            Console.WriteLine("===========================================");
            var maxElementIndexInLow = GetMaxElementBySnakeDiagonal(array, 0, n, n, m);
            Console.WriteLine($"Biggest element of the low part is: {maxElementIndexInLow.Value}[{maxElementIndexInLow.X}, {maxElementIndexInLow.Y}]");

            var maxIndexesInHigh = GetMaxIndexesBySnakeHorizontal(array, maxElementIndexInLow.Value, n, n);
            DrawListOfIndexes(maxIndexesInHigh);
        }

        private static ArrayIndex GetMaxElementBySnakeDiagonal(int[,] array, int zeroX, int zeroY, int sizeX, int sizeY)
        {
            var internalArray = CopyLowPart(array, zeroX, zeroY, sizeX, sizeY);
            var internalSizeX = sizeX - zeroX;
            var internalSizeY = sizeY - zeroY;

            var index = new ArrayIndex(-10000, 0, 0);

            var i = 0;
            var j = internalSizeY - 1;
            var counter = 0;
            var direction = Direction.Current;
            while (counter < internalSizeX * internalSizeY)
            {
                switch (direction)
                {
                    case Direction.Current:
                        {
                            counter++;
                            index = CheckMaxElement(index, internalArray, i, j);                            
                            direction = Direction.ShiftRight;
                            break;
                        }
                    case Direction.ShiftRight:
                        {
                            if (i == internalSizeX - 1)
                                j--;
                            else
                                i++;
                            counter++;
                            index = CheckMaxElement(index, internalArray, i, j);                            
                            direction = Direction.GoLeft;
                            break;
                        }
                    case Direction.ShiftLeft:
                        {
                            if (j == 0)
                                i++;
                            else
                                j--;
                            counter++;
                            index = CheckMaxElement(index, internalArray, i, j);                            
                            direction = Direction.GoRight;                            
                            break;
                        }
                    case Direction.GoLeft:
                        {
                            i--;
                            j--;
                            counter++;
                            index = CheckMaxElement(index, internalArray, i, j);
                            
                            if (i == 0 || j == 0)
                                direction = Direction.ShiftLeft;
                            else
                                direction = Direction.GoLeft;
                            
                            break;
                        }
                    case Direction.GoRight:
                        {
                            i++;
                            j++;
                            counter++;
                            index = CheckMaxElement(index, internalArray, i, j);                            
                            if (i == internalSizeX - 1 || j == internalSizeY - 1)
                                direction = Direction.ShiftRight;
                            else
                                direction = Direction.GoRight;
                            
                            break;
                        }
                }
            }

            return index;
        }

        private static ArrayIndex CheckMaxElement(ArrayIndex index, int[,] internalArray, int i, int j)
        {
            var el = internalArray[i, j];
            return (el > index.Value) ? new ArrayIndex(el, i, j) : index;
        }

        private static List<ArrayIndex> GetMaxIndexesBySnakeHorizontal(int[,] array, int max, int sizeX, int sizeY)
        {
            var results = new List<ArrayIndex>();
            var j = sizeY - 1;            
            while (j >= 0)
            {
                var isEven = (j + 1) % 2 == 0;
                if (isEven)
                {
                    for (int i = sizeX - 1; i >= 0; i--)
                    {
                        if (array[i, j] > max)
                            results.Add(new ArrayIndex(array[i, j], i, j));
                    }
                }
                else
                {
                    for (int i = 0; i < sizeX; i++)
                    {
                        if (array[i, j] > max)
                            results.Add(new ArrayIndex(array[i, j], i, j));
                    }
                }
                j--;
            }

            return results;
        }

        private static int[,] CopyLowPart(int[,] array, int zeroX, int zeroY, int sizeX, int sizeY)
        {
            var internalSizeX = sizeX - zeroX;
            var internalSizeY = sizeY - zeroY;
            var returnArray = new int[internalSizeX, internalSizeY];

            for (int j = zeroY; j < sizeY; j++)
            {
                for (int i = zeroX; i < sizeX; i++)
                    returnArray[i - zeroX, j - zeroY] = array[i, j];
            }

            return returnArray;
        }

        private static void DrawArray(int [,] array,int sizeX, int sizeY)
        {
            for(int j = 0; j< sizeY; j++)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    Console.Write($"{array[i, j]}\t");
                }
                Console.WriteLine();
            }
        }

        private static void DrawListOfIndexes(List<ArrayIndex> listOfIndexes)
        {
            if (listOfIndexes?.Count > 0)
            {
                foreach (var item in listOfIndexes)
                    Console.Write($"[{item.X},{item.Y}]: {item.Value}\t");
                Console.WriteLine();
            }
            else
                Console.WriteLine("In high part of array are no elements, which are bigger, then max of the low part of array");
        }
    }

    internal class ArrayIndex
    {
        public int Value { get; private set; }
        internal int X { get; private set; }
        internal int Y { get; private set; }

        internal ArrayIndex(int value, int x, int y)
        {
            this.Value = value;
            this.X = x;
            this.Y = y;
        }
    }
}
