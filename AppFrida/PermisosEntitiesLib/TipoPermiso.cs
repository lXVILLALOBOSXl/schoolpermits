using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PermisosEntitiesLib
{
    [Table("tipoPermiso")]
    public partial class TipoPermiso
    {
        public TipoPermiso()
        {
            Permisos = new HashSet<Permiso>();
        }

        [Key]
        [Column("idTipoPermiso")]
        public long IdTipoPermiso { get; set; }
        [Required]
        [Column("descripcionPermiso", TypeName = "VARCHAR (30)")]
        public string DescripcionPermiso { get; set; }

        [InverseProperty(nameof(Permiso.TipoPermisoNavigation))]
        public virtual ICollection<Permiso> Permisos { get; set; }
    }
}
