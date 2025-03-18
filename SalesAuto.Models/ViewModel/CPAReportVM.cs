using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{   
    public class CPAReportVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int STT0 { get; set; }
        public int STT1 { get; set; }
        public int STT2 { get; set; }
        public string GroupTitle1 { get; set; }
        public string GroupTitle2 { get; set; }
        public string GroupTitle3 { get; set; }
        public string GroupTitle4 { get; set; }
        public string BenhVien { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public long SoLuong { get; set; }
    }
}
