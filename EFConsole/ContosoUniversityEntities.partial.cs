using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConsole
{
    public partial class ContosoUniversityEntities : DbContext
    {
        public override int SaveChanges()
        {
            //配合Program.cs中『自製log』一起看。
            var entries = this.ChangeTracker.Entries();
            foreach (var entity in entries)
            {
                switch (entity.State)
                {
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Added:
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Modified:
                        if (entity.Entity is Course)
                        {
                            entity.CurrentValues.SetValues(new
                            {
                                ModifiedOn = DateTime.Now
                            });
                        }
                        break;
                    default:
                        break;
                }                
            }

            return base.SaveChanges();
        }
    }
}
