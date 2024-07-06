using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashGo.Core.Models;

namespace HashGo.Domain.DataContext
{
    public class TenantConnectItem : HashGoConnectItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime LastUpdatedTime { get; set; } = DateTime.Now;

    }
}
