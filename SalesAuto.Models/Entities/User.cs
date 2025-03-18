using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    [Keyless]
    public class User: IdentityUser<Guid>
    {
    }
}
