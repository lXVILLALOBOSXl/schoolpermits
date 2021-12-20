using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PermisosEntitiesLib
{
    [Table("areaTrabajo")]
    public partial class AreaTrabajo
    {
        public AreaTrabajo()
        {
            Empleados = new HashSet<Empleado>();
        }

        [Key]
        [Column("idAreaEmpleado")]
        public long IdAreaEmpleado { get; set; }
        [Required]
        [Column("descripcionArea", TypeName = "VARCHAR (40)")]
        public string DescripcionArea { get; set; }

        [InverseProperty(nameof(Empleado.AreaTrabajoNavigation))]
        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
