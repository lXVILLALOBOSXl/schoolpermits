using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace PermisosEntitiesLib
{
    [Table("permiso")]
    public partial class Permiso
    {
        [Key]
        [Column("folio")]
        public long Folio { get; set; }
        [Column("empleado")]
        public long Empleado { get; set; }
        [Required]
        [Column("fechaElaboracion", TypeName = "DATE")]
        public string FechaElaboracion { get; set; }
        [Required]
        [Column("fechaJustificacionInicio", TypeName = "DATE")]
        public string FechaJustificacionInicio { get; set; }
        [Required]
        [Column("fechaJustificacionFin", TypeName = "DATE")]
        public string FechaJustificacionFin { get; set; }
        [Column("horaInicio", TypeName = "TIME")]
        public string? HoraInicio { get; set; }
        [Column("horaFin", TypeName = "TIME")]
        public string? HoraFin { get; set; }
        [Column("estadoPermiso")]
        public long EstadoPermiso { get; set; }
        [Column("tipoPermiso")]
        public long TipoPermiso { get; set; }

        [ForeignKey(nameof(Empleado))]
        [InverseProperty("Permisos")]
        public virtual Empleado EmpleadoNavigation { get; set; }
        [ForeignKey(nameof(EstadoPermiso))]
        [InverseProperty("Permisos")]
        public virtual EstadoPermiso EstadoPermisoNavigation { get; set; }
        [ForeignKey(nameof(TipoPermiso))]
        [InverseProperty("Permisos")]
        public virtual TipoPermiso TipoPermisoNavigation { get; set; }
    }
}
