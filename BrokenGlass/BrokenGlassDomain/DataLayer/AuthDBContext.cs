using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BrokenGlassDomain.DataLayer
{
    public class AuthDBContext : IdentityDbContext<IdentityUser>
    {
        public AuthDBContext() : base("IdentityDbContext")
        {
            
        }
    }
}
