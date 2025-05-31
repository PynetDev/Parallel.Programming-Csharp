using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public class TaskParallelism
    {
        #region Task Basic Demo
        public void BasicTaskDemo()
        {
            // We can create task in 2 ways 
            //1. Using Task.Factory.StartNew method
            //2. Using Task constructor and Start method

            Console.WriteLine("Basic Task Demo:");
            Console.WriteLine("----------------------------------------------------");

            // Using Task.Factory.StartNew method
            Task.Factory.StartNew(() => Console.WriteLine("Hello from Task.Factory!"));

            // Using Task constructor and Start method
            Task task = new Task(() => Console.WriteLine("Hello from Task!"));
            task.Start(); // Run the task asynchronously
            task.Wait(); // Wait for the task to complete

            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("");
        }
        #endregion

        #region Task instatiation with state objects
        public void TaskWithStateObject()
        {
            Console.WriteLine("Task with State Object Demo:");
            Console.WriteLine("----------------------------------------------------");
            // Create a task that takes a state object
            var task1 = Task.Factory.StartNew(Method1,"Hello World...!");
            task1.Wait(); // Wait for the task to complete

            var task2 =Task.Factory.StartNew(state=>Method2("Hello World...!"), "Welcome");
            Console.WriteLine(task2.AsyncState);
            task2.Wait(); // Wait for the task to complete
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("");
        }
        private void Method1(object state) { Console.WriteLine(state); }
        private void Method2(string message) { Console.WriteLine(message); }
        #endregion

        #region Task Creation with TaskCreationOptions
        public void TaskCreationOptionsDemo()
        {
            Console.WriteLine("Task Creation with TaskCreationOptions Demo:");
            Console.WriteLine("----------------------------------------------------");
            // Create a task with TaskCreationOptions
            
            Task parent = Task.Factory.StartNew(() => // Parent Task
            {
                Console.WriteLine($"Parent Task is running on thread,{Thread.CurrentThread.ManagedThreadId}");
                // None: Default behavior (task continuation runs inline on the same thread)
                Task.Factory.StartNew(() => // Deatatched Task
                {
                    Console.WriteLine($"Detatched task running on thread,{Thread.CurrentThread.ManagedThreadId}");
                });

                Task.Factory.StartNew(() => // Task
                {
                    Console.WriteLine($"With fairness, task started running on thread ,{Thread.CurrentThread.ManagedThreadId}");
                },TaskCreationOptions.PreferFairness);

                Task.Factory.StartNew(() => // Child Task
                {
                    Console.WriteLine($"With AttachedToParent, Child task started running on thread ,{Thread.CurrentThread.ManagedThreadId}");
                }, TaskCreationOptions.AttachedToParent);

                Task.Factory.StartNew(() => // With DenyChildAttach, Task act as detached task
                {
                    Console.WriteLine($"With DenyChildAttach, task started as a Detatched task running on thread ,{Thread.CurrentThread.ManagedThreadId}");
                }, TaskCreationOptions.DenyChildAttach);

                Task.Factory.StartNew(() => // With HideScheduler
                {
                    Console.WriteLine($"With HideScheduler, task started running on thread ,{Thread.CurrentThread.ManagedThreadId}");
                }, TaskCreationOptions.HideScheduler);

                Task.Factory.StartNew(() => // With RunContinuationsAsynchronously
                {
                    Console.WriteLine($"With RunContinuationsAsynchronously, task started running on thread ,{Thread.CurrentThread.ManagedThreadId}");
                }, TaskCreationOptions.RunContinuationsAsynchronously);

                Task.Factory.StartNew(() => // LongRunning Child task
                {
                    Console.WriteLine($"Long running child task started. running on thread ,{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000); // Simulate a long-running task
                    Console.WriteLine("Long running child task completed");
                }, TaskCreationOptions.LongRunning).Wait();

            });
            parent.Wait(); // Wait for the parent task to complete
            Console.WriteLine("All tasks completed.");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("");
        }
        #endregion

        #region Task Waiting Techniques
        public void TaskWaitingTechniques()
        {
            Console.WriteLine("Task Waiting Techniques Demo: Example 1(Task.Wait & Task.Result)");
            Console.WriteLine("----------------------------------------------------------------");
            // calling Task's Wait method
            var task1=Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Hello World");
            });
            task1.Wait(); // Wait for the task to complete
            Console.WriteLine("Task 1 completed");

            //Accessing Task's 'Result' property in the case of Task<TResult>
            Task<string> task2 = Task.Factory.StartNew(()=>
            {
                return "Task 2 Result";
            });
            Console.WriteLine(task2.Result); // Wait for the task to complete and get the result
            Console.WriteLine("Hello World");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Task Waiting Techniques Demo: Example 2(Task.WaitAll)");
            Console.WriteLine("----------------------------------------------------------------");
            Task<string> task3 = Task.Factory.StartNew(() =>
            {
                Task.Delay(2000).Wait();
                return"Hello";
            });

            Task<string> task4 = Task.Factory.StartNew(() =>
            {
                Task.Delay(1000).Wait();
                return "World";
            });

            Task<string> task5 = Task.Factory.StartNew(() =>
            {
                Task.Delay(3000).Wait();
                return "!";
            });

            Task<string> task6 = Task.Factory.StartNew(() =>
            {
                Task.Delay(3000).Wait();
                return "Welcome";
            });

            // Wait for all tasks to complete
            Task.WaitAll(task3, task4, task5, task6);

            //print the results
            Console.WriteLine($"{task3.Result} {task4.Result} {task5.Result} {task6.Result}");
            Console.WriteLine("All tasks completed.");

            Console.WriteLine("");
            Console.WriteLine("Task Waiting Techniques Demo: Example 3(Task.WaitAny)");
            Console.WriteLine("----------------------------------------------------------------");
            Task<string> task7 = Task.Factory.StartNew(() =>
            {
                Task.Delay(2000).Wait();
                return "Hello";
            });

            Task<string> task8 = Task.Factory.StartNew(() =>
            {
                Task.Delay(1000).Wait();
                return "World";
            });

            Task<string> task9 = Task.Factory.StartNew(() =>
            {
                Task.Delay(3000).Wait();
                return "!";
            });

            Task<string> task10 = Task.Factory.StartNew(() =>
            {
                Task.Delay(3000).Wait();
                return "Welcome";
            });
            int index = Task.WaitAny(task7, task8, task9, task10);
            switch (index)
            {
                case 0:
                    Console.WriteLine($"Task 7 completed: {task7.Result}");
                    break;
                case 1:
                    Console.WriteLine($"Task 8 completed: {task8.Result}");
                    break;
                case 2:
                    Console.WriteLine($"Task 9 completed: {task9.Result}");
                    break;
                case 3:
                    Console.WriteLine($"Task 10 completed: {task10.Result}");
                    break;
            }
            Console.WriteLine("Completed");
            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("");
            Console.WriteLine("Task Waiting Techniques Demo: Example 4(Task.WhenAll)");
            Console.WriteLine("----------------------------------------------------------------");
            Task<string> task11 = Task.Factory.StartNew(() =>
            {
                Task.Delay(2000).Wait();
                return "Hello";
            });

            Task<string> task12 = Task.Factory.StartNew(() =>
            {
                Task.Delay(1000).Wait();
                return "World";
            });

            Task<string> task13 = Task.Factory.StartNew(() =>
            {
                Task.Delay(3000).Wait();
                return "!";
            });

            Task<string> task14 = Task.Factory.StartNew(() =>
            {
                Task.Delay(3000).Wait();
                return "Welcome";
            });
            Task.WhenAll(task11, task12, task13, task14);

            Console.WriteLine($"Task11 result: {task11.Result}");
            Console.WriteLine($"Task12 result: {task12.Result}");
            Console.WriteLine($"Task13 result: {task13.Result}");
            Console.WriteLine($"Task14 result: {task14.Result}");
            Console.WriteLine("Completed.");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
        }
        #endregion

        #region Task Cancellation with Cancellation Token
        public void TaskCancellationWithToken()
        {
            Console.WriteLine("Efficient Task Cancellation with Cancellation Token Demo");
            Console.WriteLine("----------------------------------------------------------------");

            //Step 1: Instance creation of CancellationTokenSource Class
            var cancellationTokenSource =new CancellationTokenSource();

            //Step 2: Accessing CancellationToken
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            //Step 3: Starting a task and check for cancellation request
            Task task = Task.Factory.StartNew(() => {
                //some long running task
                for (int i = 0; i < 10000; i++) {
                    Console.WriteLine(i);
                    Thread.Sleep(100); //simulate work
                    cancellationToken.ThrowIfCancellationRequested(); // check for cancellation request
                }
            },cancellationToken);

            //Step 4: Cancelling the CancellationTokenSource
            cancellationTokenSource.CancelAfter(500); // Cancels after 500 milliseconds

            try
            {
                //Step 5: Catch an Aggregate Exception and check the inner exception for detecting a canceled task
                task.Wait();
            }
            catch (AggregateException ex)
            {
                if(ex.InnerException is OperationCanceledException)
                {
                    Console.WriteLine("Task was cancelled.");
                }
                else
                {
                    throw; // Rethrow the exception if it's not a cancellation exception
                }
                
            }
            Console.WriteLine("Method Completed.");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
        }
        #endregion

        #region Task Exception Handling Example 1
        public void TaskExceptionHandling1()
        {
            Console.WriteLine("Task Exception Handling Demo1:");
            Console.WriteLine("----------------------------------------------------------------");
            int divisor = 0;
            Task<int> task = Task.Factory.StartNew(() =>
            {
                // This will throw a DivideByZeroException
                return 10 / divisor;
            });
            try
            {
                Console.WriteLine(task.Result);
            }
            catch (AggregateException aex)
            {
                // Handle the exception
                Console.WriteLine("An exception occurred: " + aex.InnerException.Message); // Exception: Divided by zero
            }
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
        }

        #endregion

        #region Task Exception Handling Example 2
        public void TaskExceptionHandling2()
        {
            Console.WriteLine("Task Exception Handling Demo2:");
            Console.WriteLine("----------------------------------------------------------------");
            CallMainAsyncMethod();
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
        }
        private async Task CallMainAsyncMethod()
        {
            try
            {
                var task1 = MyAsyncMethod1();
                var task2 = MyAsyncMethod2();
                await Task.WhenAll(task1, task2);
            }
            catch (Exception ex)
            {

                Console.Write(ex);
            }
        }
        private async Task MyAsyncMethod1()
        {
            int divisor = 0;
            Console.WriteLine($"Divisor Value:{divisor}");
        }
        private async Task MyAsyncMethod2()
        {
            // This will throw a DivideByZeroException
            int divisor = 0;
            int result = 10 / divisor;
            Console.WriteLine(result);
        }
        #endregion

        #region Task Exception Handling Example 3
        public async Task TaskExceptionHandling3()
        {
            Console.WriteLine("Task Exception Handling Demo3:");
            Console.WriteLine("----------------------------------------------------------------");
            Task aggregationTask = null;
            try
            {
                var task1 = MyAsyncMethod3();
                var task2 = MyAsyncMethod4();
                aggregationTask = Task.WhenAll(task1, task2);
                await aggregationTask;
            }
            catch (Exception ex)
            {
                if (aggregationTask.Exception?.InnerExceptions != null && aggregationTask.Exception.InnerExceptions.Any())
                {
                    foreach (var innerEx in aggregationTask.Exception.InnerExceptions)
                    {
                        Console.WriteLine(innerEx.Message);
                    }
                }
            }
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
        }
        
            
        
        private async Task MyAsyncMethod3()
        {
            int divisor = 0;
            int result = 5 / divisor;
            Console.WriteLine(result);
        }
        private async Task MyAsyncMethod4()
        {
            // This will throw a DivideByZeroException
            int divisor = 0;
            int result = 10 / divisor;
            Console.WriteLine(result);
        }

        #endregion

        #region Task Exception Handling Example 4
        public void TaskExceptionHandling4()
        {
            Console.WriteLine("Task Exception Handling Demo4:");
            Console.WriteLine("----------------------------------------------------------------");
            Task.Factory.StartNew(() =>
            {
                try
                {
                    // Simulating some operation
                    Console.WriteLine("Detached task is running...");

                    // Simulating an exception
                    throw new InvalidOperationException("Detatched task encountered an error");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Detatched task caught an exception: {ex.Message}");
                }
            });

            // Task waited upon a  with a timeout
            Task<int> calc = Task.Factory.StartNew(() => {
                int divisor = 0;
                // simulating a long running calculation
                Task.Delay(2000).Wait();
                return 7 / divisor; // This will throw a DivideByZeroException
            });
            try
            {
                // Waiting for a task with a timeout
                if(calc.Wait(1000)) // Wait for 1 second
                {
                    // If the task completes within the timeout, print the result
                    Console.WriteLine($"Calculation result: {calc.Result}");
                }
                else
                {
                    // If the task does not complete within the timeout, print a message
                    Console.WriteLine("Calculation timed out.");
                }

            }
            catch (AggregateException aex)
            {
                Console.WriteLine("Task with timeout caught an exception : " + aex.InnerException.Message);
                throw;
            }
            Console.WriteLine("Program Completed");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
        }
        #endregion

        #region Task Continuation Example
        public void TaskContinuation()
        {
            Console.WriteLine("Task Continuation Example:");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Task and their continuations might run on different threads.");
            Console.WriteLine("----------------------------------------------------------------");
            Task task1 = Task.Factory.StartNew(()=>Console.WriteLine("First task.."));
            Task task2 = task1.ContinueWith(ant => Console.WriteLine("..second task"));
            task2.Wait();
            Console.WriteLine("");
            Console.WriteLine("Efficient Approach: Task and their continuations will run on same threads when we use TaskContinuationOptions");
            Task task3 = Task.Factory.StartNew(() => Console.WriteLine("First task.."));
            Task task4 = task1.ContinueWith(ant => Console.WriteLine("..second task"),TaskContinuationOptions.ExecuteSynchronously);
            task2.Wait();
            Console.WriteLine("");
            Console.WriteLine("Chaining tasks with data");
            Task.Factory.StartNew(() => 20)
                .ContinueWith(ant=>ant.Result+5)
                .ContinueWith(ant=>Math.Sqrt(ant.Result))
                .ContinueWith(ant=>Console.WriteLine($"Calculated Result: {ant.Result}")).Wait();
            Console.WriteLine("");
            Console.WriteLine("Multiple Anticedent Tasks:");
            Task task5 = Task.Factory.StartNew(() => Console.WriteLine("Task 5 complete"));
            Task task6 = Task.Factory.StartNew(() => Console.WriteLine("Task 6 complete"));
            var continuation = Task.Factory.ContinueWhenAll(new[] { task5, task6 }, tasks => Console.WriteLine("All tasks done"));
            Console.WriteLine("");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
        }
        #endregion

    }

}

