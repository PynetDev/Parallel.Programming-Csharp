using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public class DataPrallelism

    {
        #region Parallel.Invoke()
        public void ParalleInvokeMethod()
        {
            // Example of using Parallel.Invoke
            Console.WriteLine("Parallel.Invoke Demo:");
            Console.WriteLine("---------------------------");
            Parallel.Invoke(
                () => Console.WriteLine(Task1()),
                () => Console.WriteLine(Task2()),
                () => Console.WriteLine(Task3())
            );
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
        }

        private int Task1()
        {
            // Basic logic: sum numbers from 1 to 10
            int sum = 0;
            for (int i = 1; i <= 10; i++)
            {
                sum += i;
            }
            return sum;
        }

        private string Task2()
        {
            // Basic logic: concatenate strings in a list
            var words = new List<string> { "Hello", "from", "Task2" };
            return string.Join(" ", words);
        }

        private bool Task3()
        {
            // Basic logic: check if current minute is odd
            return DateTime.Now.Minute % 2 != 0;
        }
        #endregion

        #region Parallel.For()
        public void ParallelForMethod()
        {
            // Example of using Parallel.For
            Console.WriteLine("Parallel.For Demo:");
            Console.WriteLine("---------------------------");
            Parallel.For(0, 50, i =>
            {
                PrintOutput(i);
            });
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
        }
        private void PrintOutput(int i)
        {
            
            Console.WriteLine($"Square of {i}:{Math.Pow(i,2)}");
        }
        #endregion

        #region Parallel.ForEach()
        public void ParallelForEachMethod()
        {
            // Example of using Parallel.ForEach
            Console.WriteLine("Parallel.ForEach Demo:");
            Console.WriteLine("---------------------------");
            int vowelCount = 0;
            int consonantCount = 0;
            object lockObj = new object();

            Parallel.ForEach("Parallel.ForEach Demo Example...!", c =>
            {
                if (char.IsLetter(c))
                {
                    if ("AEIOUaeiou".Contains(c))
                    {
                        lock (lockObj)
                        {
                            vowelCount++;
                        }
                        Console.WriteLine($"Vowel found: {c}");
                    }
                    else
                    {
                        lock (lockObj)
                        {
                            consonantCount++;
                        }
                        Console.WriteLine($"Consonant found: {c}");
                    }
                }
            });

            Console.WriteLine($"Vowel count: {vowelCount}");
            Console.WriteLine($"Consonant count: {consonantCount}");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
        }
        public void ParallelForNestedLoops()
        {
            // Example of using Parallel.For with nested loops (three scenarios)
            Console.WriteLine("Parallel.For Nested Loops Demo (Three Scenarios):");
            Console.WriteLine("---------------------------");

            int size = 10;
            int[,] matrix1 = new int[size, size];
            int[,] matrix2 = new int[size, size];
            int[,] matrix3 = new int[size, size];

            var sw1 = System.Diagnostics.Stopwatch.StartNew();
            // Scenario 1: Both outer and inner loops are parallel
            Parallel.For(0, size, i =>
            {
                Parallel.For(0, size, j =>
                {
                    matrix1[i, j] = i * j;
                });
            });
            sw1.Stop();

            Console.WriteLine("Scenario 1: Both loops parallel");
            PrintMatrix(matrix1);
            Console.WriteLine($"Execution Time: {sw1.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------");

            var sw2 = System.Diagnostics.Stopwatch.StartNew();
            // Scenario 2: Outer loop parallel, inner loop normal
            Parallel.For(0, size, i =>
            {
                for (int j = 0; j < size; j++)
                {
                    matrix2[i, j] = i * j;
                }
            });
            sw2.Stop();

            Console.WriteLine("Scenario 2: Outer loop parallel, inner loop normal");
            PrintMatrix(matrix2);
            Console.WriteLine($"Execution Time: {sw2.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------");

            var sw3 = System.Diagnostics.Stopwatch.StartNew();
            // Scenario 3: Both loops normal
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix3[i, j] = i * j;
                }
            }
            sw3.Stop();

            Console.WriteLine("Scenario 3: Both loops normal");
            PrintMatrix(matrix3);
            Console.WriteLine($"Execution Time: {sw3.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
        }

        private void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region Indexed Parallel.ForEach()
        public void IndexedParallelForEachMethod()
        {
            // Example of using Parallel.ForEach with indexed access
            Console.WriteLine("Indexed Parallel.ForEach Demo:");
            Console.WriteLine("------------------------------");
            var numbers = Enumerable.Range(1, 10).ToList();
            Parallel.ForEach(numbers, (number, state, index) =>
            {
                Console.WriteLine($"Index: {index}, Number: {number}, Square: {number * number}");
            });
            Console.WriteLine("------------------------------");
            Console.WriteLine("");
        }
        #endregion

        #region Parallel Loop State
        public void ParallelLoopStateMethod()
        {
            // Example of using Parallel Loop State
            Console.WriteLine("Parallel Loop State Demo:");
            Console.WriteLine("---------------------------");
            Console.WriteLine("Parallel Loop State With State.Break():");
            Console.WriteLine("");
            Parallel.ForEach("Parallel Loop State, Demo!".ToCharArray(), (c, state) =>
            {
                if (c == ',')
                {
                    state.Break(); // Breaks the loop for current iteration
                }
                Console.Write(c);
            });
            Console.WriteLine("\n\nParallel Loop State With State.Stop():");
            Console.WriteLine("");
            Parallel.ForEach("Parallel Loop State, Demo!".ToCharArray(), (c, state) =>
            {
                if (c == ',')
                {
                    state.Stop(); // Stops the entire loop execution
                }
                Console.Write(c);
            });
            Console.WriteLine("");
            Console.WriteLine("Parallel Loop State, Demo!".Length);
        }
        #endregion

        #region Harnessing Local Variables in Parallel Loops
        public void ParallelLoopLocalVariables()
        {
            // Example of using local variables in parallel loops
            Console.WriteLine("Parallel Loop Local Variables Demo:");
            Console.WriteLine("---------------------------");
            double total = 0;
            Parallel.For(1, 10000000, 
                () => 0.0, 
                (i, state, localTotal) =>
            {
                localTotal += Math.Sqrt(i); // Local variable for each thread
                return localTotal;
            }, localTotal =>
            {
                total += localTotal; // Combine results from all threads
            });
            Console.WriteLine($"Total sum of square roots: {total}");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");
        }
        #endregion
    }

}
