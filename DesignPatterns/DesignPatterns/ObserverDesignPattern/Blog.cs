using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.DesignPatterns.ObserverDesignPattern
{
    public class Blog
    {
        private readonly List<ISubscriber> _subscribers = new();
        public void Subscribe(ISubscriber subscriber)
        {
            _subscribers.Add(subscriber);
            Console.WriteLine("Subscriber added.");
        }
        public void Unsubscribe(ISubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
            Console.WriteLine("Subscriber removed.");
        }
        public void PublishNewArticle(string articleTitle)
        {
            Console.WriteLine($"New article published: {articleTitle}");
            foreach (var subscriber in _subscribers)
            {
                subscriber.Update(articleTitle);
            }
        }
    }
}
