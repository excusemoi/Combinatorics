using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IntSameValues cmp = new IntSameValues();
            int key = 0, n = 0, k = 0;
            try{
                Console.WriteLine("Введите количество элементов массива чисел");
                n = int.Parse(Console.ReadLine());
                if (n <= 0)
                {
                    Console.WriteLine("Некорректный размер массива");
                    return;
                }
                int[] array = new int[n];
                Console.WriteLine("Введите элементы массива чисел");
                for (int i = 0; i < n; i++)
                    array[i] = int.Parse(Console.ReadLine());
                while (key != 5)
                {
                    PrintInstruction();
                    key = int.Parse(Console.ReadLine());
                    switch (key)
                    {
                        case 1:
                        {
                            Console.WriteLine("Введите k");
                            k = int.Parse(Console.ReadLine());
                            List<int[]> res = array.Cmb(k, cmp, true).ToList();
                            Console.WriteLine("Результат:\n");
                            PrintListOfListsInt(res);
                            Console.WriteLine(
                                "Количество элементов искомого множества = С(k, n+k-1) = C({0}, {1}) = {2}", k,
                                n + k - 1,
                                Calculations.Combination(n + k - 1, k));
                            break;
                        }
                        case 2:
                        {
                            List<int[]> res = array.Subsets(cmp).ToList();
                            Console.WriteLine("Результат:");
                            PrintListOfListsInt(res);
                            Console.WriteLine(
                                "Количество элементов искомого множества = 2^(Количество элементов массива) = 2^{0} = {1}",
                                n, Math.Pow(2, n));
                            break;
                        }
                        case 3:
                        {
                            List<int[]> res = array.Prm(cmp).ToList();
                            Console.WriteLine("Результат:");
                            PrintListOfListsInt(res);
                            Console.WriteLine(
                                "Количество элементов искомого множества = (Количество элементов массива)! = {0}! = {1}",
                                n, Calculations.Factorial(n));
                            break;
                        }
                        case 4:
                        {
                            PrintInstruction();
                            break;
                        }
                        case 5:
                        {
                            Console.WriteLine("Пока..");
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        static void PrintInstruction()
        {
            Console.WriteLine("1.Сочетания по k с повторениями");
            Console.WriteLine("2.Булеан (множество всех подмножеств)");
            Console.WriteLine("3.Перестановки");
            Console.WriteLine("4.Инструкция");
            Console.WriteLine("5.Выход");
        }

        static void PrintListOfListsInt(List<int[]> l)
        {
            for (int i = 0; i < l.Count(); i++)
            {
                Console.Write("{0}{1}", i + 1, ": [");
                if(l.ElementAt(i).Count() == 0)
                    Console.WriteLine(']');
                for (int j = 0; j < l.ElementAt(i).Length; j++)
                    Console.Write("{0}{1}",l.ElementAt(i)[j], (j == (l.ElementAt(i).Length - 1)) ? ("]\n") : (", "));
            }
        }
    }
  
    public static class Extensions
    {
        public static IEnumerable<T[]> Cmb<T>(this IEnumerable<T> source, int k, IEqualityComparer<T> cmp, bool repeats)
        {
            if (!source.SequenceEqual(source.Distinct(cmp))) 
                throw new ArgumentException("the collection contains the same elements\n");
            List<T> src = source.ToList(), o = new List<T>();
            List<T[]> combList = new List<T[]>();
            CmbCalc(combList, src, o, k, 0, source.Count(), repeats);
            return combList.AsEnumerable();
        }

        public static void CmbCalc<T>(List<T[]> combList, List<T> source, List<T> o, int k, int i, int n, bool repeats)
        {
            if (o.Count == k)
            {
                combList.Add(o.ToArray());
                return;
            }

            for (int j = i; j < n; j++)
            {
                if(repeats == false && o.Contains(source[j])) 
                    continue;
                o.Add(source[j]);
                CmbCalc<T>(combList, source, o, k, j, n, repeats);
                o.RemoveAt(o.Count-1);
            }
        }
        public static IEnumerable<T[]> Subsets<T>(this IEnumerable<T> source, IEqualityComparer<T> cmp)
        {
            if (!source.SequenceEqual(source.Distinct(cmp)))
                throw new ArgumentException("the collection contains the same elements\n");
            List<T> src = source.ToList(), o = new List<T>();
            List<T[]> sbstsList = new List<T[]>();
            int length = source.Count();
            for(int i = 0; i < Math.Pow(2, length); i++)
                CmbCalc(sbstsList, src, o, i, 0, length, false);
            return sbstsList.AsEnumerable();
        }
        public static IEnumerable<T[]> Prm<T>(this IEnumerable<T> source, IEqualityComparer<T> cmp)
        {
            if (!source.SequenceEqual(source.Distinct(cmp)))
                throw new ArgumentException("the collection contains the same elements\n");
            List<T> src = source.ToList(), o = new List<T>();
            List<T[]> prmList = new List<T[]>();
            PrmCalc(prmList, src, 0, source.Count());
            return prmList.AsEnumerable();
        }

        public static void PrmCalc<T>(List<T[]> prmList, List<T> source, int ind, int n)
        {
            if (ind == n)
            {
                prmList.Add(source.ToArray());
                return;
            }
            for (int i = ind; i < n; i++)
            {
                Swap(source, ind, i);
                PrmCalc<T>(prmList, source, ind + 1, n );
                Swap(source, i, ind);
            }
        }

        public static void Swap<T>(List<T> l, int ind, int ind_)
        {
            T tmp = l[ind];
            l[ind] = l[ind_];
            l[ind_] = tmp;
        }

    }
    public class IntSameValues : EqualityComparer<int>
    {
        public override bool Equals(int a, int b)
        {
            return (a == b);
        }

        public override int GetHashCode(int num)
        {
            return num.GetHashCode();
        }
    }
    public class Calculations
    {
        public static int Factorial(int n)
        {
            if (n < 1)
                return 0;
            else if (n == 1)
                return 1;
            else return n * Factorial(n - 1);
        }
        public static int Combination(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }
    }
}