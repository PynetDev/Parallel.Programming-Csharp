using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ParallelProgramming
{
    public class ConcurrentCollections
    {
        #region Concurrent Queue
        public T[] ConcurrentQueueDemo<T>(IEnumerable<T> itemsToEnqueue)
        {
            // Create a thread-safe queue to hold items
            var queue = new ConcurrentQueue<T>();

            // A list to hold all the enqueue tasks
            var tasks = new List<Task>();

            // Enqueue all items in parallel using Task.Run
            foreach (var item in itemsToEnqueue)
            {
                tasks.Add(Task.Run(() => queue.Enqueue(item))); // Safely enqueue from multiple threads
            }

            // Wait for all enqueue operations to complete
            Task.WaitAll(tasks.ToArray());

            // Use a thread-safe collection to store dequeued results
            var results = new ConcurrentBag<T>();

            // Clear the task list for reuse in dequeue phase
            tasks.Clear();

            // Spawn parallel dequeue tasks equal to the number of items
            for (int i = 0; i < itemsToEnqueue.Count(); i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    // Try to dequeue an item; add it to results if successful
                    if (queue.TryDequeue(out T? result))
                    {
                        results.Add(result); // ConcurrentBag is thread-safe for parallel adds
                    }
                }));
            }

            // Wait for all dequeue operations to complete
            Task.WaitAll(tasks.ToArray());

            // Return the collected results as an array (order is not guaranteed)
            return results.ToArray();
        }



        #endregion

        #region Concurrent Queue Demo2
        public void ConcurrentQueueDemo2()
        {
            ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
            Console.WriteLine("First In First Out - Logging System");
            Console.WriteLine("-----------------------------------------------");

            Task task1 = Task.Run(() => { 
              for(int i = 0; i < 5; i++)
                {
                    string logEntry = $"Log {i} generated at {DateTime.Now}";
                    queue.Enqueue(logEntry);
                    Console.WriteLine($"Enqueued (In): {logEntry}");
                    Task.Delay(1000).Wait();
                }
            });

            Task task2 = task1.ContinueWith(t => {
                Task.Delay(500).Wait();
                while (!queue.IsEmpty)
                {
                    if(queue.TryDequeue(out string? result))
                    {
                        Console.WriteLine($"Processed (Out): {result}");
                    }
                }
            });

            Task.WaitAll(task1,task2);
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");

        }
        #endregion

        #region Concurrent Stack
        public void ConcurrentStackDemo()
        {
            ConcurrentStack<string> stack = new ConcurrentStack<string>();
            Console.WriteLine("Last In First Out - Undo Functionality");
            Console.WriteLine("-----------------------------------------------");

            Task task1 = Task.Run(() => {
                for (int i = 0; i < 5; i++)
                {
                    
                    stack.Push($"Action {i}");
                    Console.WriteLine($"Performed: Action {i}");
                    
                }
            });

            Task task2 = task1.ContinueWith(t => {
                
                while (stack.TryPop(out string? result))
                {  
                   Console.WriteLine($"Undo: {result}");
                }
            });

            Task.WaitAll(task1, task2);
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }
        #endregion


    }
}
