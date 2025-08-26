using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AdvSwProject.Models;

namespace AdvSwProject.Data.Interceptors
{
    public class softDeleteInterceptor : SaveChangesInterceptor
    {
       


        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var dbc = eventData.Context;
            if (dbc is null)
            {
                return new InterceptionResult<int>();
            }
            var data = dbc.ChangeTracker.Entries<BaseEntity>();
            foreach (var entity in data)
            {
                if(entity.State is EntityState.Deleted)
                {
                    entity.State = EntityState.Modified;
                    entity.Entity.DeletedDate = DateTime.UtcNow;
                    entity.Entity.IsDelete = true;
                }

            }
            return base.SavingChanges(eventData, result);
        }

       
    }
}
