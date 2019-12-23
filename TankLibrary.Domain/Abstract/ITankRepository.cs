using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary.Domain.Entities;

namespace TankLibrary.Domain.Abstract
{
    public interface ITankRepository
    {
        IEnumerable<Tank> Tanks { get; }
        void Update(Tank tank);
        Tank Add(Tank tank);
        void Delete(int id);
        int ResetData(int idMax = 0);
    }
}
