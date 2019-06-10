using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практика10
{

    public class NewListException : ApplicationException
    {
        // Реализуем стандартные конструкторы,
        public NewListException() : base() { }
        public NewListException(string str) : base(str) { }
        // Переопределяем метод ToStringO для класса NewListException.
        public override string ToString()
        {
            return Message;
        }
    }

    public class NewListEnumerator<T>
    {
        NewList<T> items;
        int position;

        public NewListEnumerator(NewList<T> ml)
        {
            items = ml;
            Reset();
        }

        public object Current { get { return items[position]; } }

        public bool MoveNext()
        {
            if (position < items.Count - 1)
            {
                position++;
                return true;
            }
            else return false;
        }

        public void Reset()
        {
            position = -1;
        }

    }

    public struct MyElem<TKey, TValue>
    {
        public TKey power;
        public TValue rate;

        public MyElem(TKey n, TValue m)
        {
            power = n;
            rate = m;
        }

        public TKey Power { get { return power; } }
        public TValue Rate { get { return rate; } }
    }
    public class NewList<T>
    {
        public T[] list;
        public int count = 0;
        public int position;
        public int Count { get { return count; } }


        public NewList()
        {
            list = new T[0];
        }

        public NewListEnumerator<T> GetEnumerator()
        {
            return new NewListEnumerator<T>(this);
        }

        public virtual T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                if (index >= 0 && index < list.Length)
                    list[index] = value;
                else
                {
                    throw new NewListException("В коллекции нет элемента с таким индексом");
                }
            }
        }

        public void Add(T k)
        {
            T[] arr = new T[Count + 1];
            for (int i = 0; i < list.Length; i++)
            {
                arr[i] = list[i];
            }
            arr[list.Length] = k;
            list = arr;
            count++;
        }


    }
    class Program
    {
        static int ReadIntNumber(string stringForUser, int left, int right)
        {
            bool okInput = false;
            int number = -100;
            do
            {
                Console.WriteLine(stringForUser);
                try
                {
                    string buf = Console.ReadLine();
                    number = Convert.ToInt32(buf);
                    if (number >= left && number < right) okInput = true;
                    else
                    {
                        Console.WriteLine("Неверно введено число!");
                        okInput = false;
                    }
                }
                catch
                {
                    Console.WriteLine("Неверно введено число!");
                    okInput = false;
                }
            } while (!okInput);
            return number;
        }
        static double FindY(int x, NewList<MyElem<int, int>> n)
        {
            double y = 0;
            for (int i = 0; i < n.Count; i++)
            {
                double s = Convert.ToDouble(n[i].Power);
                y += n[i].Rate * Math.Pow(x, s);
            }
            return y;

        }

        static Random rnd = new Random();
        static void Main(string[] args)
        {
            NewList<MyElem<int, int>> list = new NewList<MyElem<int, int>>();
            bool ok = true;
            //построчное чтение 
            try
            {
                StreamReader f = new StreamReader("text.txt");
                string s;
                int i = 0;
                while ((s = f.ReadLine()) != null)
                {
                    if (s.Length == 2 && i != 100)
                    {
                        try
                        {
                            i++;
                            int number = Convert.ToInt32(s);
                            int pow = number / 10;
                            int rate = number % 10;
                            if (rate != 0) list.Add(new MyElem<int, int>(pow, rate));
                        }
                        catch (FormatException)
                        {
                            ok = false;
                            break;
                        }
                    }
                    else
                    {
                        ok = false;
                        break;
                    }
                }
                f.Close();
                if (ok)
                {
                    int x = -100;
                    NewList<int> X = new NewList<int>();
                    do
                    {
                        x = ReadIntNumber("Введите x:", -10, 11);
                        X.Add(x);
                    } while (x != 0);
                    double y = 0;
                    foreach (int x1 in X)
                    {
                        y = FindY(x1, list);
                        Console.WriteLine("x = {0} y = {1}", x1, y);
                    }
                }
                else Console.WriteLine("Информация в файле указана неправильно!");
            }
            catch (FileNotFoundException e)
            {
                ok = false;
                Console.WriteLine(e.Message);
                Console.WriteLine("Проверьте правильность имени файла!");
            }
            catch (Exception e)
            {
                ok = false;
                Console.WriteLine("Error: " + e.Message);
                return;
            }


            Console.ReadKey();

        }

    }
}
