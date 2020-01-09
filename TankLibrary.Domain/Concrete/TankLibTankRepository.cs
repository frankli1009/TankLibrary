using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary.Domain.Abstract;
using TankLibrary.Domain.Entities;

namespace TankLibrary.Domain.Concrete
{
    public class TankLibTankRepository : ITankRepository
    {
        private TankLibDbContext context = new TankLibDbContext();

        public IEnumerable<Tank> Tanks
        {
            get
            {
                return context.Tanks;
            }
        }

        public void Update(Tank entity)
        {
            var DbSet = context.Set<Tank>();
            //DbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Tank Add(Tank entity, int maxRecordCount = 0)
        {
            if (maxRecordCount > 0) 
            {
                if (Tanks.Count() >= maxRecordCount)
                {
                    throw new MaxRecordCountReachedException();
                }
            }
            entity.Id = 0;
            var DbSet = context.Set<Tank>();
            Tank result = DbSet.Add(entity);
            context.SaveChanges();

            return result;
        }

        public void Delete(int id)
        {
            var DbSet = context.Set<Tank>();
            Tank tank = DbSet.Find(id);
            DbSet.Remove(tank);
            context.SaveChanges();
        }

        public int ResetData(int idMax = 0)
        {
            return context.Database.ExecuteSqlCommand("delete from Tank where Id > @idmax",
                new SqlParameter("@idmax", idMax));
        }
    }
}
