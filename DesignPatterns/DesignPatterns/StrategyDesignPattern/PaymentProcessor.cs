using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.DesignPatterns.StrategyDesignPattern
{
    public class PaymentProcessor
    {
        private readonly IPaymentStrategy _paymentStrategy;
        public PaymentProcessor(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }
        public void ProcessPayment(decimal amount)
        {
            _paymentStrategy.Pay(amount);
        }
    }
}
