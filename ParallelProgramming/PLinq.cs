using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public class PLinq
    {
        static int staticvar = 5;
        [ThreadStatic]
        static int threadstaticvar = 10;
        static ThreadLocal<int> threadlocalvar = new ThreadLocal<int>(() => 20);
        #region PLINQ Demo
        public void PLinqDemo()
        {
            Console.WriteLine("Demo for Plinq in C#");
            Console.WriteLine("--------------------");
            var numbers = Enumerable.Range(1, 1000000).ToList();

            //use plinq to filter odd numbers
            var query = numbers.AsParallel()
                .Where(n => n % 2 == 1)
                .ToList();
            Console.WriteLine($"Count of odd numbers: {query.Count()}");
            Console.WriteLine();

            Console.WriteLine("Input character set: uvwxyz");
            Console.WriteLine("Unordered output: ");
            Console.WriteLine("uvwxyz".AsParallel().Select(c => char.ToUpper(c)).ToArray());
            Console.WriteLine("Ordered output: ");
            Console.WriteLine("uvwxyz".AsParallel().AsOrdered().Select(c => char.ToUpper(c)).ToArray());


        }
        #endregion

        #region PLINQ With Merge Options
        public void PLinqWithMergeOptions()
        {
            Console.WriteLine("PLINQ's With Merge Options Demo in C#:");
            Console.WriteLine("--------------------------------------");
            int[] numbers1 = Enumerable.Range(1, 10000).ToArray();
            // Using WithMergeOptions to control buffering behavior
            var result = numbers1.AsParallel()
            //buffer size will be automatically determined by the system
            .WithMergeOptions(ParallelMergeOptions.AutoBuffered) // Default one (ParallelMergeOptions.Deafult)
            .Where(x => x % 2 == 0)
            .Select(x => x)
            .ToArray();

            Console.WriteLine("Even Numbers between 1 and 10000: ");
            foreach (var item in result)
            {
                Console.Write(item + "\n");
            }
        }
        #endregion

        #region Degree of Parallelism
        public void PLinqWithDegreeOfParallelism()
        {
            Console.WriteLine("PLINQ's WithDegree Of Parallelism Demo in C#: ");
            Console.WriteLine("----------------------------------------------");
            string[] websites =
            {
              "www.Google.com",
              "www.wikipedia.org",
              "www.yahoo.com",
              "www.youtube.com"
            };
            var results = websites.AsParallel() // for parallel execution of LINQ queries
                          .WithDegreeOfParallelism(4) //Gives hint to run with a maximum of four parallel tasks.
                          .Select(site =>
                          {
                              Ping objPing = new Ping();
                              PingReply reply = objPing.Send(site);
                              return new
                              {
                                  site,
                                  Result = reply.Status,
                                  Time = reply.RoundtripTime
                              };
                          });
            foreach (var result in results)
            {
                Console.WriteLine($"Website: {result.site}, Result: {result.Result}, Time: {result.Time}ms");
            }
        }
        #endregion

        #region Functional Purity
        public void PLinqWithFunctionalPurity()
        {
            Console.WriteLine("Demo of Functional Purity in C# PLINQ:");
            Console.WriteLine("--------------------------------------");
            WithoutFunctionalPurity();
            WithFunctionalPurity();
        }
        private void WithoutFunctionalPurity()
        {
            // The following query multiplies each element by its position.
            // Given an input of Enumerable.Range(0,5), it should output squares.
            int position = 0;
            var query = from n in Enumerable.Range(0, 5).AsParallel()
                        select n * position++;
            Console.WriteLine("Result without functional purity: ");
            // Execute the query and print the results
            foreach (var result in query)
            {
                Console.WriteLine(result);
            }
            Console.WriteLine($"position: {position}");
        }
        private void WithFunctionalPurity()
        {
            // The following query multiplies each element by its position.
            // Making the Code Functionally Pure
            var query = Enumerable.Range(0, 5).AsParallel().Select((n, position) => n * position);
            Console.WriteLine("Result with functional purity: ");
            // Execute the query and print the results
            foreach (var result in query)
            {
                Console.WriteLine(result);
            }

        }
        #endregion

        #region Static Variable
        public void PLinqStaticVariable()
        {
            Console.WriteLine("Static Variable Demo in Multithreading Context: ");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"Main Thread - Initial value: {staticvar}");
            // Start a new thread and modify the num value I
            Thread thread1 = new Thread(() =>
            {
                staticvar = 3;
                Console.WriteLine($"New Thread - Updated value: {staticvar}");
            });
            thread1.Start();
            thread1.Join(); // Wait for the new thread to finish
            Console.WriteLine($"Main Thread - Final value: {staticvar}");


            Console.WriteLine("[ThreadStatic] Attribute Demo in Multithreading Context: ");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"Main Thread - Initial value: {threadstaticvar}");
            // Start a new thread and modify the num value I
            Thread thread2 = new Thread(() =>
            {
                threadstaticvar = 3;
                Console.WriteLine($"New Thread - Updated value: {threadstaticvar}");
            });
            thread2.Start();
            thread2.Join(); // Wait for the new thread to finish
            Console.WriteLine($"Main Thread - Final value: {threadstaticvar}");

            Console.WriteLine("Thread Local Class Demo in Multithreading Context: ");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"Main Thread - Initial value: {threadlocalvar}");
            // Start a new thread and modify the num value I
            Thread thread3 = new Thread(() =>
            {
                threadlocalvar.Value = 3;//if we comment this line , it will print 20 as initial value where in thread static attribute this line will give 0
                Console.WriteLine($"New Thread - Updated value: {threadlocalvar}");
            });
            thread3.Start();
            thread3.Join(); // Wait for the new thread to finish
            Console.WriteLine($"Main Thread - Final value: {threadlocalvar}");

        }
        #endregion

        #region PLINQ Cancellation Using Break
        public void PLinqCancellationUsingBrakeStatement()
        {
            Console.WriteLine("Technique 1: PLINQ Cancellation Demo using break statement");
            Console.WriteLine("----------------------------------------------------------");
            IEnumerable<int> numbers = Enumerable.Range(1, 1000);
            // PLINQ query to double each number
            var query = from num in numbers.AsParallel().AsOrdered()
                        select num * 2;
            foreach (var result in query)
            {
                if (result > 100) // Cancel if result exceeds 100
                    break; // EXIT the Loop
                Console.WriteLine(result);
            }
        }
        #endregion

        #region PLINQ Cancellation Using Cancellation Token
        public void PLinqCancellationUsingCancellationToken()
        {
            Console.WriteLine("Technique 2: PLINQ Cancellation Demo using cancellationToken");
            Console.WriteLine("------------------------------------------------------------");
            // Generating a sequence of numbers from 1 to 10000
            IEnumerable<int> numbers = Enumerable.Range(1, 10000);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            // PLINQ query to filter even numbers
            var evenNumbersQuery = numbers.AsParallel()
            .WithCancellation(cancellationToken)
            .Where(n => {
                Thread.Sleep(10);  // Slow down query to allow cancellation
                return n % 2 == 0;
            });
            // Starting a new thread to cancel the query after 50 milliseconds
            new Thread(() =>
            {
                Thread.Sleep(1);
                cancellationTokenSource.Cancel(); // Cancel the PLINQ query
            }).Start();
            try
            {
                // Start executing the query and storing even numbers in an array
                int[] evenNumbers= evenNumbersQuery.ToArray();
                // We'll never reach this line because the query will be canceled
                Console.WriteLine("Even Numbers: " + string.Join(", ", evenNumbers));
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Query cancelled.");
            }
        }
        #endregion

        #region PLINQ using output centric
        public void PLinqUsingOutputCentric()
        {
            // Simple PLINQ Query
            var simplePlinqQuery = "uvwxyz".AsParallel().Select(c => char.ToUpper(c)).ToArray();
            Console.WriteLine("simplePlingQuery Output :");
            foreach (char c in simplePlinqQuery)
            {
               Console.WriteLine(c);
            }
            //
            Console.WriteLine("");
            Console.WriteLine("Optimizing simplePlinqQuery using Output-Centric way:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Optimized Query Output :");
            "uvwxyz".AsParallel().Select(c => char.ToUpper(c)).ForAll(Console.WriteLine);
        }
        #endregion

        #region PLINQ using Input Centric
        public void PLinqUsingInputCentric()
        {
            Console.WriteLine("");
            Console.WriteLine("Optimizing PLINQ Query using Input-Centric way:");
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("1. Using Hash Paritioning...");
            List<int> numbers = Enumerable.Range(1, 100).ToList(); // Sample list of numbers
                                                                   // Define the number of partitions
            int numPartitions = 4;
            // Perform hash partitioning using PLINQ
            var partitions = numbers.AsParallel()
            .GroupBy(n => Math.Abs(n.GetHashCode()) % numPartitions)
            .Select(group => group.ToList())
            .ToList();
            // Display the partitions
            for (int i = 0; i < partitions.Count; i++)
            {
                Console.WriteLine($"Partition {i}: {string.Join(", ", partitions[i])}");
            }
            Console.WriteLine("");

            // Example of using Range Partitioning
            Console.WriteLine("2. Using Range Paritioning...");
            var rangePartitioningPLINQQueryOutput = ParallelEnumerable.Range(1, 1000).Select(i => Math.Pow(i, 2)).Sum();
            Console.Write($" Sum:{ rangePartitioningPLINQQueryOutput}");
            Console.WriteLine("");

            //Example of using Chunk Partitioning
            Console.WriteLine("");
            Console.WriteLine("3. Using Chunk Paritioning...");
            int[] digits = { 1, 2, 3, 4, 5, 6, 7 };
            // Creating a partitioner with chunk partitioning I
            var plinqQuery = Partitioner.Create(digits, true)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount) // Using all available processors
            .Select(chunk => ProcessChunk(chunk));
            foreach (var result in plinqQuery)
            {
                Console.WriteLine($"{result}");
            }
            
        }
        private int ProcessChunk(int chunk)
        {
            // Simulating processing of a chunk
            return chunk * chunk; // just returning the Square numbers
        }
        #endregion

        #region PLINQ Aggregation
        public void PLinqAggregation()
        {
            Console.WriteLine("");
            Console.WriteLine("PLINQ Parallel Aggreation Demo...");
            Console.WriteLine("---------------------------------");
            // Example data
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Console.WriteLine("---------------------------------");
            Console.WriteLine("SECTION-1: ");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("PLINQ works well with SUM, Average, Min and Max: Demo");
            Console.WriteLine(".....................................................");
            // Sum
            int sum = numbers.AsParallel().Sum();
            Console.WriteLine($"Sum: {sum}");

            // Average
            double average = numbers.AsParallel().Average();
            Console.WriteLine($"Average: {average}");

            // Min
            int min = numbers.AsParallel().Min();
            Console.WriteLine($"Min: {min}");

            // Max
            int max = numbers.AsParallel().Max();
            Console.WriteLine($"Max: {max}");

            Console.WriteLine("---------------------------------");
            Console.WriteLine("SECTION-2: ");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Aggregate function with or without seed value: Demo");
            Console.WriteLine(".....................................................");

            int aggregateSumWithSeedValue=numbers.Aggregate(0,(total,n)=>total+n);
            Console.WriteLine($"AggregateSumWithSeedValue:{aggregateSumWithSeedValue}"); //0+1+2....+10=55

            int aggregateSumWithOutSeedValue = numbers.Aggregate((total, n) => total + n);
            Console.WriteLine($"AggregateSumWithOutSeedValue:{aggregateSumWithOutSeedValue}");//1+2+...+10=55

            int aggregateMultiplyWithSeedValue = numbers.Aggregate(0, (total, n) => total * n);
            Console.WriteLine($"AggregateSumWithSeedValue:{aggregateMultiplyWithSeedValue}"); //0*1*2*...10=0

            int aggregateMultiplyWithOutSeedValue = numbers.Aggregate((total, n) => total * n);
            Console.WriteLine($"AggregateSumWithOutSeedValue:{aggregateMultiplyWithOutSeedValue}"); //1*2*...10=36288300

            Console.WriteLine("---------------------------------");
            Console.WriteLine("SECTION-3(a): ");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Parallel Aggregation with PLINQ Aggregate function: Demo");
            Console.WriteLine(".....................................................");

            int result = numbers.AsParallel()
                .Aggregate(
                 ()=>0,                                          //Seed Factory: intialize the accumulator to 0
                 (localTotal,n)=>localTotal+n,                  //Update Accumulator Func: add each number to the accumulator
                 (mainTotal,localTotal)=>mainTotal+localTotal, //Combine Accumulator Func: Combine Accumulators
                 finalResult => finalResult                    //result Selector: return the final result
                );
            Console.WriteLine($"Sum Result using aggregate function:{ result}");

            Console.WriteLine("---------------------------------");
            Console.WriteLine("SECTION-3(b): ");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Parallel Aggregation with PLINQ Aggregate function: Demo");
            Console.WriteLine(".....................................................");

            string strText = "PLINQ is a parallel implementation of LINQ in C#";
            Console.WriteLine($"Input String: {strText}");
            int[] letterFrequencies=strText.AsParallel()
                .Aggregate(                                  //perform parallel processing with plinq and aggregate
                    () => new int[26],                      // Seed Factory: Initialize an array of 26 integers to hold letter frequencies
                    (localFreq, c) =>                      // Update Accumulator Func: Update the frequency of each letter
                    {
                        char upperC = char.ToUpper(c);    // Convert character to uppercase
                        if (char.IsLetter(upperC))        // check if the character is letter
                        {
                            int index = upperC - 'A';     // Calculate index for the letter in the aplhabet (0 for 'A', 1 for 'B', ..., 25 for 'Z')
                            localFreq[index]++;           // Increment the corresponding element in the local accumulator array
                        }
                        return localFreq;
                    },
                    (mainFreq, localFreq) =>                // Combine Accumulator Func: Combine the local accumulator arrays into the main accumulator array

                        mainFreq.Zip(localFreq, (main, local) => main + local).ToArray(), // use Zip to combine the two arrays element-wise

                    finalResult => finalResult // Result Selector: Return the final frequency array
                );

            Console.WriteLine("Letter Frequencies:");
            for(int i = 0; i < letterFrequencies.Length; i++)
            {
                char letter = (char)('A' + i); // Convert index back to corresponding uppercase letter
                Console.WriteLine($"{letter}: {letterFrequencies[i]}"); //display the letter and its frequency
            }

            Console.WriteLine("");
        }
        #endregion
    }
}
