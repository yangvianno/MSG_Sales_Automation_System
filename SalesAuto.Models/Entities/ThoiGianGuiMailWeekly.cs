using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ThoiGianGuiMailWeekly
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Tuan { get; set; }
        public int Nam { get; set; }
        public DateTime NgayGui { get; set; }
        public KetQuaGuiMail KetQua { get; set; }
    }
}
