using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.BestTech
{
    public class Category
    {
        public string name { get; set; }
        public string code { get; set; }
        public object categoryImage { get; set; }
        public double monthlyQtyLimit { get; set; }
        public bool isDeleted { get; set; }
        public object deleterUserId { get; set; }
        public object deletionTime { get; set; }
        public object lastModificationTime { get; set; }
        public object lastModifierUserId { get; set; }
        public DateTime creationTime { get; set; }
        public int creatorUserId { get; set; }
        public int id { get; set; }
    }

    public class ResultList<T>
    {
        public List<T> items { get; set; }
    }

    public class Base<T>
    {
        public ResultList<T> result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class SalesOrderResponse
    {
        public TransactionDetails result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class TransactionDetails
    {
        public int id { get; set; }
        public string transactionNo { get; set; }
    }
}
