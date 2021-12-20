using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PermisosEntitiesLib
{
    [Table("plantel")]
    public partial class Plantel
    {
        public Plantel()
        {
            Empleados = new HashSet<Empleado>();
        }

        [Key]
        [Column("idTipoPlantel")]
        public long IdTipoPlantel { get; set; }
        [Required]
        [Column("descripcionPlantel", TypeName = "VARCHAR (30)")]
        public string DescripcionPlantel { get; set; }

        [InverseProperty(nameof(Empleado.PlantelNavigation))]
        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
