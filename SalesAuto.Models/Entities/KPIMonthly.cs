using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class KPIMonthly
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public long ActualDigitalCost { get; set; }
        public long ActualBranding { get; set; }
        public long BudgetDigitalCost { get; set; }
        public string? MaBenhVien { get; set; }
    }
}
