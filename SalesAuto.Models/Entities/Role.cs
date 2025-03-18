using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class Role : IdentityRole<Guid>
    {
        [MaxLength(250)]
        public string MoTa { get; set; }
    }
}
