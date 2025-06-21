using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.DesignPatterns.FactoryDesignPattern
{
    public class PushNotification: INotification
    {
        public void send(string to, string message)
        {
            Console.WriteLine($"Push notification sent to {to} with message: {message}");
        }
    }
   
}
