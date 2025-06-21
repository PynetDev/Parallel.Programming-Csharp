using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.DesignPatterns.ObserverDesignPattern
{
    public class MobileSubscriber: ISubscriber
    {
        private readonly string _name;
        public MobileSubscriber(string name)
        {
            _name = name;
        }
        public void Update(string article)
        {
            Console.WriteLine($"{_name} got mobile notification: {article}");
        }
    }
    
}
