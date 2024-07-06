using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashGo.Core.Models;
using HashGo.Core.Models.Ticket;

namespace HashGo.Wpf.App.Payment
{
    public interface IPaymentProcessor
    {
        decimal Preprocess(FlyTicket ticket, PaymentType paymentType, decimal dueAmount, decimal tenderedAmount);
        PaymentProcessResult Process(FlyTicket ticket, PaymentType paymentType, PaymentProcessResult processResult);
    }
}
