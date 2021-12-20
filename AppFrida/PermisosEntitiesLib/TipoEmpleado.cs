using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PermisosEntitiesLib
{
    [Table("tipoEmpleado")]
    public partial class TipoEmpleado
    {
        public TipoEmpleado()
        {
            Empleados = new HashSet<Empleado>();
        }

        [Key]
        [Column("idTipoEmpleado")]
        public long IdTipoEmpleado { get; set; }
        [Required]
        [Column("descripcionEmpleado", TypeName = "VARCHAR (30)")]
        public string DescripcionEmpleado { get; set; }

        [InverseProperty(nameof(Empleado.TipoEmpleadoNavigation))]
        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
