using System;
using System.Collections.Generic;

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

            var n = Convert.ToInt32(inputSize);
            var m = 2 * n;

            //var array = FillArrayByRandom(_rnd, n, m);
            var array = FillArrayByRandom(_rnd, n, m);
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
            while (counter <= internalSizeX * internalSizeY)
            {
                switch (direction)
                {
                    case Direction.Current:
                        {
                            index = CheckMaxElement(index, internalArray, i, j);
                            counter++;
                            direction = Direction.ShiftRight;
                            i++;
                            break;
                        }
                    case Direction.ShiftRight:
                        {
                            index = CheckMaxElement(index, internalArray, i, j);
                            counter++;
                            direction = Direction.GoLeft;
                            i--;
                            j--;
                            break;
                        }
                    case Direction.ShiftLeft:
                        {
                            index = CheckMaxElement(index, internalArray, i, j);
                            counter++;
                            direction = Direction.GoRight;
                            i++;
                            j++;
                            break;
                        }
                    case Direction.GoLeft:
                        {
                            index = CheckMaxElement(index, internalArray, i, j);
                            counter++;
                            if (i == 0 || j == 0)
                                direction = Direction.ShiftLeft;
                            else
                            {
                                i--;
                                j--;
                            }
                            break;
                        }
                    case Direction.GoRight:
                        {
                            index = CheckMaxElement(index, internalArray, i, j);
                            counter++;
                            if (i == internalSizeX - 1 || j == internalSizeY - 1)
                                direction = Direction.ShiftRight;
                            else
                            {
                                i++;
                                j++;
                            }
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

        private static void SwapElementsD(int[,] array, int i, int j)
        {
            var temp = array[i, j];
            array[i, j] = array[j, i];
            array[j, i] = temp;
        }

        private static void SwapElementsH(int[,] array, int i, int j, int sizeY)
        {
            var jk = sizeY - j - 1;
            var temp = array[i, j];
            array[i, j] = array[i, jk];
            array[i, jk] = temp;
        }

        private static int[,] FillArrayByRandom(Random rnd, int sizeX, int sizeY)
        {
            var a = new int[sizeX, sizeY];
            for (int j = 0; j < sizeY; j++)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    a[i, j] = rnd.Next(-100, +100);
                }
            }

            return a;
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
