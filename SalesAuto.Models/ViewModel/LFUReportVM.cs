using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class LFUReportVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public long SoLuong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int STT1 { get; set; }
        public int STT2 { get; set; }
    }
}
