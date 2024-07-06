using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.Ticket
{
    public class PaymentProcessResult
    {
        public PaymentProcessResult(decimal amount, decimal tenderedAmount, string description)
        {
            DueAmount = amount;
            TenderedAmount = tenderedAmount;
            Description = description;
            CanContinueProcessing = true;
        }

        public bool CanContinueProcessing { get; set; }

        public string Description { get; set; }

        public decimal DueAmount { get; set; }

        public decimal TenderedAmount { get; set; }

        public PaymentType ChangePaymentType { get; set; }
    }
}
