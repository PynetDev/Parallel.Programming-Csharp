namespace DesignPatterns.DesignPatterns.SingleTonDesignPattern
{
    //Without Thread Safety
    public sealed class SingleTon
    {
        //private variable
        private static readonly SingleTon _instance = new SingleTon();

        //private constructor to prevent instantiation
        private SingleTon() { }

        //public property to get the instance
        public static SingleTon Instance { get { return _instance; } }
    }

    //With Thread Safety
    public sealed class ThreadSafeSingleTon
    {
        // private variable to hold the instance
        private static volatile ThreadSafeSingleTon? _instance;

        // private object for locking to ensure thread safety
        private static readonly object _lockObject = new object();

        // private constructor to prevent instantiation
        private ThreadSafeSingleTon(){ }

        // public property to get the instance
        public static ThreadSafeSingleTon Instance {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new ThreadSafeSingleTon();
                            
                        }
                    }
                }
                return _instance;
            }
        }
    }

    //Lazy<T>
    public sealed class LazySingleTon
    {
        // private variable to hold the instance using Lazy<T>
        private static readonly Lazy<LazySingleTon> _instance =
            new Lazy<LazySingleTon>(() => new LazySingleTon());

        // private constructor to prevent instantiation
        private LazySingleTon(){ }

        // public property to get the instance
        public static LazySingleTon Instance => _instance.Value;
    }
}
