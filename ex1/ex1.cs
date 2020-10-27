using System;
using System.Collections.Generic;

namespace Ex1
{
    /* 1
     * a. Find the Pythagorean triple a<b<c such that a + b + c = 1000 (knowing that it is unique):
     * b.build a function receiving a natural number n and prints all the Pythagorean triples a<b<c
     * for which a + b + c = n.
     * Print the output for triples with the sums: 1000, 12, 2000.
     * 
     * 2
     * Build a function receiving a collection of strings and prints for each string how many times
     * it appears in this collection: [string, count].
     * 
     * 3
     * How many letters (excluding spaces etc.) are needed to write all the numbers from 1 to 100
     * (excluding 100)?
     */
    class Program
    {
        public class Q1
        {
            public void A()
            {
                /* Boundaries of for loops: First, a cannot be larger than 333, since if so b must be at least 334 
                 * and c must be at least 335, summing up to at least 1001. Second, b cannot be 500 or larger, 
                 * from a similar reason (if so, than b + c > 1000). 
                 * Therefore, the for loops can iterate over fewer numbers than expected.
                 */
                for (int a = 1; a < 333; a++)
                {
                    for (int b = a + 1; b < 500; b++)
                    {
                        int c = 1000 - a - b;
                        if (b < c && a * a + b * b == c * c)
                        {
                            Console.WriteLine($"{a},{b},{c}");
                            return;
                        }
                    }
                }                
            }
            public void B(int n)
            {
                /* Similar reasons as in 1a lead to smaller boundaries for a and b:
                 * a must be smaller than n / 3, b must be smaller than n / 2.
                 */
                float fln = (float)n;
                for (int a = 1; a < fln / 3; a++)
                {
                    for (int b = a + 1; b < fln / 2; b++)
                    {
                        int c = n - b - a;
                        if (b < c && a * a + b * b == c * c)
                        {
                            Console.WriteLine($"{a},{b},{c}");
                        }
                    }
                }
            }
        }
        public class Q2
        {
            public Q2(string[] strs)
            {
                if (strs == null) return;
                Dictionary<string, int> counts = new Dictionary<string, int>();
                foreach (string s in strs)
                {
                    if (counts.ContainsKey(s))
                        counts[s] += 1;
                    else
                        counts[s] = 1;
                }
                foreach (KeyValuePair<string, int> sc in counts)
                {
                    Console.WriteLine($"[{sc.Key}, {sc.Value}]");
                }
            }
        }

        public class Q3
        {
            readonly int[] unityDigitLetterCount = new int[] { 0, 3, 3, 5, 4, 4, 3, 5, 5, 4 }; // zero isn't counted, "one" has three letters, etc.
            readonly int[] tensDigitLetterCount = new int[] { 0, 3, 6, 6, 5, 5, 5, 7, 6, 6 }; // zero isn't counted, "ten" has three letters, etc.
            readonly int[] teenCounter = new int[] { 3, 6, 6, 8, 8, 7, 7, 9, 9, 8 }; // counting ten to nineteen exceptionally.
            public Q3()
            {
                int letterCount = 0;
                for (int i=1; i < 100; i++)
                {
                    letterCount += Num2LetterCount(i);
                }
                Console.WriteLine($"To write 1 till 99, one needs {letterCount} letters");
            }
            int Num2LetterCount(int num)
            {
                int numTensDig = num / 10;
                int numUnityDig = num - numTensDig * 10;
                if (numTensDig == 1)
                {
                    return teenCounter[numUnityDig];
                }
                else
                {
                    return unityDigitLetterCount[numUnityDig] + tensDigitLetterCount[numTensDig];
                }

            }
        }
        static void Main()
        {
            Console.WriteLine("Q1");
            Q1 q1 = new Q1();
            Console.WriteLine("A");
            q1.A();
            Console.WriteLine("\nB");
            Console.WriteLine("1000:");
            q1.B(1000);
            Console.WriteLine("\n12:");
            q1.B(12);
            Console.WriteLine("\n2000:");
            q1.B(2000);
            Console.WriteLine("\nQ2");
            string[] line = new string[] { "Mamas", "Empire", "Mamas", "39", "Empire", "Mamas", "39", "Respect" };
            Q2 q2 = new Q2(line);
            Console.WriteLine("\nQ3");
            Q3 q3 = new Q3();
        }
    }
}
