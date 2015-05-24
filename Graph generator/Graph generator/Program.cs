using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = Console.ReadLine().Split();
            if (input.Count() == 0) return;
            if (input[0] == "web")
            {
                if (input.Count() < 3)
                {
                    Console.WriteLine("Usage: web edges depth [hole]");
                    return;
                }
                int n, d, h;
                try
                {
                    n = Convert.ToInt32(input[1]);
                    d = Convert.ToInt32(input[2]);
                    if (input.Count() == 4) h = Convert.ToInt32(input[3]);
                    else h = 0;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect value.");
                    return;
                }

                int v, e;
                v = 1 + n * d;
                e = 2 * n * d;
                if (h > 0)
                {
                    v -= 1 + n * (h - 1);
                    e -= n + 2 * n * (h - 1);
                }
                Console.WriteLine(v);
                Console.WriteLine(e);

                for (int i = 0; i <= Math.Min(d - h, d - 1); i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        Console.WriteLine(String.Format("{0} {1}", i * n + j, i * n + j % n + 1));
                        if (i != d - h)
                        {
                            if (i != d - 1)
                                Console.WriteLine(String.Format("{0} {1}", i * n + j, (i + 1) * n + j));
                            else
                                Console.WriteLine(String.Format("{0} {1}", i * n + j, 1 + n * d));
                        }
                    }
                }
            }
            else if (input[0] == "torus")
            {
                if (input.Count() < 3)
                {
                    Console.WriteLine("Usage: torus n N");
                    return;
                }
                int n, N;
                try
                {
                    n = Convert.ToInt32(input[1]);
                    N = Convert.ToInt32(input[2]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect value.");
                    return;
                }

                int v, e;
                v = n * N;
                e = 2 * n * N;
                Console.WriteLine(v);
                Console.WriteLine(e);

                for (int i = 0; i < N; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        Console.WriteLine(String.Format("{0} {1}", i * n + j, i * n + j % n + 1));
                        Console.WriteLine(String.Format("{0} {1}", i * n + j, ((i + 1) % N) * n + j));
                    }
                }
            }
            else if (input[0] == "klein")
            {
                if (input.Count() < 3)
                {
                    Console.WriteLine("Usage: klein n N");
                    return;
                }
                int n, N;
                try
                {
                    N = Convert.ToInt32(input[1]);
                    n = Convert.ToInt32(input[2]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect value.");
                    return;
                }

                int v, e;
                v = n * N;
                e = 2 * n * N;
                Console.WriteLine(v);
                Console.WriteLine(e);

                for (int i = 0; i < N; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        Console.WriteLine(String.Format("{0} {1}", i * n + j, i * n + j % n + 1));
                        if (i != N - 1)
                            Console.WriteLine(String.Format("{0} {1}", i * n + j, ((i + 1) % N) * n + j));
                        else
                        {
                            Console.WriteLine(String.Format("{0} {1}", i * n + j, ((i + 1) % N) * n + (n + 1 - j) % n + 1));
                            //Console.WriteLine("<" + j.ToString() + ", " + ((n + 1 - j) % n + 1).ToString() + ">");
                        }
                    }
                }
            }
            else if (input[0] == "rp")
            {
                if (input.Count() < 3)
                {
                    Console.WriteLine("Usage: rp n k");
                    return;
                }
                int n, k;
                try
                {
                    n = Convert.ToInt32(input[1]);
                    k = Convert.ToInt32(input[2]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect value.");
                    return;
                }
                Console.WriteLine(n * k);
                Console.WriteLine(n * k * 2  - 2);
                int v;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 1; j < k; j++)
                    {
                        v = i * k + j;
                        Console.WriteLine(v.ToString() + " " + (v + 1).ToString());
                    }
                }
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = 1; j <= k; j++)
                    {
                        v = i * k + j;
                        Console.WriteLine(v.ToString() + " " + (v + k).ToString());
                    }
                }
                for (int j = 1; j <= k; j++)
                {
                    v = (n - 1) * k + j;
                    Console.WriteLine(v.ToString() + " " + (k + 1 - j).ToString());
                }
                for (int i = 1; i < n - 1; i++)
                {
                    v = i * k + k;
                    Console.WriteLine(v.ToString() + " " + ((n - i - 1) * k + 1).ToString());
                }
            }
            else if (input[0] == "tree")
            {
                if (input.Count() < 2)
                {
                    Console.WriteLine("Usage: tree n");
                    return;
                }
                int n;
                try
                {
                    n = Convert.ToInt32(input[1]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect value.");
                    return;
                }
                Console.WriteLine(n);
                Console.WriteLine(n - 1);
                Random r = new Random();
                for (int i = 2; i <= n; i++)
                    Console.WriteLine(i.ToString() + " " + (r.Next(i - 1) + 1).ToString());
            }
            else if (input[0] == "cube")
            {
                if (input.Count() < 2)
                {
                    Console.WriteLine("Usage: cube n");
                    return;
                }
                int n;
                try
                {
                    n = Convert.ToInt32(input[1]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect value.");
                    return;
                }
                edges.Clear();
                printCube(n, 1);
                Console.WriteLine(2 << (n - 1));
                Console.WriteLine(edges.Count);
                foreach (var e in edges)
                    Console.WriteLine(e.Item1 + " " + e.Item2);
            }
        }
        static List<Tuple<int, int>> edges = new List<Tuple<int, int>>();
        static int printCube(int n, int nr)
        {
            if (n > 0)
            {
                int k;
                k = printCube(n - 1, nr);
                printCube(n - 1, nr + k);
                for (int i = nr; i < nr + k; i++)
                    edges.Add(new Tuple<int, int>(i, i + k));
                return 2 * k;
            }
            else return 1;
        }
    }
}