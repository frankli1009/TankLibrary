using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TankLibrary.Domain.Entities;

namespace TankLibrary.Models
{
    public class TankListViewModel
    {
        public IEnumerable<Tank> Tanks { get; set; }
        public PagingInfo PageInfo { get; set; }
        public int? CurrentStage { get; set; }
    }
}