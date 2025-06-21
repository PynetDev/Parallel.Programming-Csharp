using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.DesignPatterns.FactoryDesignPattern
{
    public interface INotification
    {
        void send(string to, string message);
    }
}
