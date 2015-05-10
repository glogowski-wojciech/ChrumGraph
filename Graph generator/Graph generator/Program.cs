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
        }
    }
}