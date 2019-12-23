using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TankLibrary.Domain.Entities
{
    public class DerivedTank: Tank
    {
        [Required]
        [MaxLength(40)]
        public new string Name {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
    }
}
