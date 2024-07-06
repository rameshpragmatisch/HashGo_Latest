using HashGo.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{

    public class Department
    {
        public string name { get; set; }
        public string code { get; set; }
        //public bool settlementPrintSummary { get; set; }
        //public bool settlementPrintDetail { get; set; }
        //public bool isAdditionalQuota { get; set; }
        //public object purchaseAccountId { get; set; }
        //public object salesAccountId { get; set; }
        //public bool isDeleted { get; set; }
        //public object deleterUserId { get; set; }
        //public object deletionTime { get; set; }
        //public object lastModificationTime { get; set; }
        //public object lastModifierUserId { get; set; }
        //public DateTime creationTime { get; set; }
        //public int creatorUserId { get; set; }
        public int id { get; set; }
    }

    public class Result
    {
        public List<Department> items { get; set; }
    }

    public class DepartmentRoot
    {
        public Result result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

}
