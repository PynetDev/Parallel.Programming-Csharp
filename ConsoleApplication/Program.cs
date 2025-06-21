using DesignPatterns.DesignPatterns.FactoryDesignPattern;
using DesignPatterns.DesignPatterns.ObserverDesignPattern;
using DesignPatterns.DesignPatterns.SingleTonDesignPattern;
using DesignPatterns.DesignPatterns.StrategyDesignPattern;
using ParallelProgramming;

#region PLinq Class Methods
//PLinq pLinq = new PLinq();

//pLinq.PLinqDemo();
//pLinq.PLinqWithMergeOptions();
//pLinq.PLinqWithDegreeOfParallelism();
//pLinq.PLinqWithFunctionalPurity();
//pLinq.PLinqStaticVariable();
//pLinq.PLinqCancellationUsingBrakeStatement();
//pLinq.PLinqCancellationUsingCancellationToken();
//pLinq.PLinqUsingOutputCentric();
//pLinq.PLinqUsingInputCentric();
//pLinq.PLinqAggregation();
#endregion

#region Data Parallelism Class Methods
//DataPrallelism dataPrallelismObj = new DataPrallelism();

//dataPrallelismObj.ParalleInvokeMethod(); 
//dataPrallelismObj.ParallelForMethod();
//dataPrallelismObj.ParallelForEachMethod();
//dataPrallelismObj.ParallelForNestedLoops();
//dataPrallelismObj.IndexedParallelForEachMethod();
//dataPrallelismObj.ParallelLoopStateMethod();
//dataPrallelismObj.ParallelLoopLocalVariables();
#endregion

#region Task Parallelism Class Methods
//TaskParallelism taskParallelismObj = new TaskParallelism();

//taskParallelismObj.BasicTaskDemo();
//taskParallelismObj.TaskWithStateObject();
//taskParallelismObj.TaskCreationOptionsDemo();
//taskParallelismObj.TaskWaitingTechniques();
//taskParallelismObj.TaskCancellationWithToken();
//taskParallelismObj.TaskExceptionHandling1();
//taskParallelismObj.TaskExceptionHandling2();
//taskParallelismObj.TaskExceptionHandling3();
//taskParallelismObj.TaskExceptionHandling4();
//taskParallelismObj.TaskContinuation();
#endregion

#region ConcurrentCollections Methods
//ConcurrentCollections concurrentCollectionsObj = new ConcurrentCollections();

////Concurrent Queue Demo 1
//var result = concurrentCollectionsObj.ConcurrentQueueDemo(Enumerable.Range(1, 10));
//Console.WriteLine("Dequeued Items: " + string.Join(", ", result));

////Concurrent Queue Demo 2
//concurrentCollectionsObj.ConcurrentQueueDemo2();

////Concurrent Stack Demo
//concurrentCollectionsObj.ConcurrentStackDemo();

#endregion

#region Design Patterns

// Singleton Design Pattern
Console.WriteLine("==== Testing Basic Singleton ====");
var singleton1 = SingleTon.Instance;
var singleton2 = SingleTon.Instance;
Console.WriteLine(ReferenceEquals(singleton1, singleton2) ? "Same instance" : "Different instances");
Console.WriteLine($"HashCode: {singleton1.GetHashCode()} == {singleton2.GetHashCode()}");

Console.WriteLine("\n==== Testing Thread-Safe Singleton ====");
var threadSafe1 = ThreadSafeSingleTon.Instance;
var threadSafe2 = ThreadSafeSingleTon.Instance;
Console.WriteLine(ReferenceEquals(threadSafe1, threadSafe2) ? "Same instance" : "Different instances");
Console.WriteLine($"HashCode: {threadSafe1.GetHashCode()} == {threadSafe2.GetHashCode()}");

Console.WriteLine("\n==== Testing Lazy<T> Singleton ====");
var lazy1 = LazySingleTon.Instance;
var lazy2 = LazySingleTon.Instance;
Console.WriteLine(ReferenceEquals(lazy1, lazy2) ? "Same instance" : "Different instances");
Console.WriteLine($"HashCode: {lazy1.GetHashCode()} == {lazy2.GetHashCode()}");


// Factory Design Pattern
Console.WriteLine("\n==== Testing Factory Design Pattern ====");
Console.WriteLine("");
Console.WriteLine("Enter notification type (email/sms/push):");
string type = Console.ReadLine()?.Trim() ?? "email";

INotification notifier = NotificationFactory.CreateNotification(type);

notifier.send("john@example.com", "Your account has been created!");
Console.WriteLine("");

//Strategy Design Pattern
Console.WriteLine("\n==== Testing Strategy Design Pattern ====");
Console.WriteLine("");
Console.WriteLine("Select Payment Method: 1. Credit Card  2. PayPal  3. Google Pay");
var input = Console.ReadLine();

IPaymentStrategy strategy = input switch
{
    "1" => new CreditCard(),
    "2" => new PayPal(),
    "3" => new GooglePay(),
    _ => throw new InvalidOperationException("Invalid payment method")
};

PaymentProcessor processor = new PaymentProcessor(strategy);
processor.ProcessPayment(1000);
Console.WriteLine("");

//Observer Design Pattern
Console.WriteLine("\n==== Testing Observer Design Pattern ====");
Console.WriteLine("");
Blog techBlog = new Blog();

var alice = new EmailSubscriber("Alice");
var bob = new MobileSubscriber("Bob");
var charlie = new EmailSubscriber("Charlie");

techBlog.Subscribe(alice);
techBlog.Subscribe(bob);

techBlog.PublishNewArticle("Observer Pattern Explained in C#");

techBlog.Subscribe(charlie);
techBlog.Unsubscribe(alice);

techBlog.PublishNewArticle("Strategy Pattern Real-Time Use Case");
Console.WriteLine("");
#endregion
