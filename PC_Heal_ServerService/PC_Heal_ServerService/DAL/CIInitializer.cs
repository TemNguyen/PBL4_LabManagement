using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PC_Heal_ServerService.DAL
{
    public class CIInitializer : DropCreateDatabaseIfModelChanges<CIDbContext>
    {
        protected override void Seed(CIDbContext context)
        {
            base.Seed(context);
        }
    }
}
