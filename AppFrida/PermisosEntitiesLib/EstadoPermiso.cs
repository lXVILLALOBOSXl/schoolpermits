using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PermisosEntitiesLib
{
    [Table("estadoPermiso")]
    public partial class EstadoPermiso
    {
        public EstadoPermiso()
        {
            Permisos = new HashSet<Permiso>();
        }

        [Key]
        [Column("idEstadoPermiso")]
        public long IdEstadoPermiso { get; set; }
        [Required]
        [Column("descripcionEstado", TypeName = "VARCHAR (30)")]
        public string DescripcionEstado { get; set; }

        [InverseProperty(nameof(Permiso.EstadoPermisoNavigation))]
        public virtual ICollection<Permiso> Permisos { get; set; }
    }
}
