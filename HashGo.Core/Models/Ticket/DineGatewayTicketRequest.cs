using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.Ticket
{
    public class DineGatewayTicketRequest
    {
        public bool claimed { get; set; }
        public string? deliveryContents { get; set; }
        public string? locationId { get; set; }
        public string? response { get; set; }
        public string? status { get; set; }
        public string? ticketNumber { get; set; }
       
    }
}
