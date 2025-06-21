using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.DesignPatterns.ObserverDesignPattern
{
    public interface ISubscriber
    {
        void Update(string article);
    }
}
