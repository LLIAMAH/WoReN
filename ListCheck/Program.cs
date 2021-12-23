using System;
using System.Collections;
using System.Linq;

namespace ListCheck
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to one directional list with head!");
            var continueMarker = true;

            var list = new CircularLinkedList();

            while (continueMarker)
            {
                Console.Write("Enter integer value (non-digit chars to exit from program): ");
                var inputValue = Console.ReadLine();
                if (inputValue != null && inputValue.Any(char.IsLetter))
                {
                    continueMarker = false;
                    Console.WriteLine("Latest list values before remove are: ");
                    Console.WriteLine(OutputList(list));
                    list.RemoveAll();
                    continue;
                }

                var newValue = Convert.ToInt32(inputValue);

                list.Add(newValue);
                Console.WriteLine("List values are: ");
                Console.WriteLine(OutputList(list));
            }
        }

        public static string OutputList(CircularLinkedList list)
        {
            var result = string.Empty;
            foreach (var item in list)
                result += $"{item}, ";

            result = result.TrimEnd(new[] { ' ', ',' });
            result += '.';

            return result;
        }
    }

    public class Node
    {
        public int Data { get; set; }
        public Node Next { get; set; }

        public Node(int data)
        {
            Data = data;
        }
    }

    public class CircularLinkedList : IEnumerable  // кольцевой связный список
    {
        private int _maxValue = int.MinValue;
        private Node _head; // головной/первый элемент
        private Node _tail; // последний/хвостовой элемент
        private int _count;  // количество элементов в списке

        // добавление элемента
        public void Add(int data)
        {
            //if(data > _maxValue)
              //  _maxValue = data;

            Node node = new Node(data);
            // если список пуст
            if (_head == null)
            {
                _maxValue = data;
                _head = node;
                _tail = node;
                _tail.Next = _head;
            }
            else
            {
                var current = _head;
                while (current.Next != _tail && current.Next.Data != _maxValue)
                {
                    current = current.Next;
                }

                if (current.Next.Data > data)
                {
                    // вставляем перед
                    var tempRef = current.Next;
                    current.Next = node;
                    node.Next = tempRef;
                }
                else
                {
                    // вставяляем после и заменяем _maxValue значение
                    _maxValue = data;
                    node.Next = _head;
                    _tail.Next = node;
                    _tail = node;
                }
            }
            _count++;
        }

        public void RemoveAll()
        {
            Node current = _head;
            Node previous = null;

            if (IsEmpty) return;

            do
            {
                // Если узел в середине или в конце
                if (previous != null)
                {
                    // убираем узел current, теперь previous ссылается не на current, а на current.Next
                    previous.Next = current.Next;

                    // Если узел последний,
                    // изменяем переменную _tail
                    if (current == _tail)
                        _tail = previous;
                }
                else // если удаляется первый элемент
                {

                    // если в списке всего один элемент
                    if (_count == 1)
                    {
                        _head = _tail = null;
                    }
                    else
                    {
                        _head = current.Next;
                        _tail.Next = current.Next;
                    }
                }

                _count--;

                previous = current;
                current = current.Next;
            } while (current != _head);
        }

        public bool IsEmpty => _count == 0;

        IEnumerator IEnumerable.GetEnumerator()
        {
            var current = _head;
            do
            {
                if (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            while (current != _head);
        }
    }
}
