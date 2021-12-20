using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PermisosEntitiesLib
{
    [Table("empleado")]
    public partial class Empleado
    {
        public Empleado()
        {
            Logins = new HashSet<Login>();
            Permisos = new HashSet<Permiso>();
        }

        [Key]
        [Column("numeroDeNomina")]
        public long NumeroDeNomina { get; set; }
        [Required]
        [Column("nombres", TypeName = "VARCHAR (40)")]
        public string Nombres { get; set; }
        [Required]
        [Column("apellidoPaterno", TypeName = "VARCHAR (20)")]
        public string ApellidoPaterno { get; set; }
        [Required]
        [Column("apellidoMaterno", TypeName = "VARCHAR (20)")]
        public string ApellidoMaterno { get; set; }
        [Required]
        [Column("curp", TypeName = "VARCHAR (18)")]
        public string Curp { get; set; }
        [Required]
        [Column("email", TypeName = "VARCHAR (40)")]
        public string Email { get; set; }
        [Column("numeroTelefonico", TypeName = "INTEGER (10)")]
        public long? NumeroTelefonico { get; set; }
        [Required]
        [Column("calle", TypeName = "VARCHAR (20)")]
        public string Calle { get; set; }
        [Column("numeroInterior", TypeName = "INTEGER (5)")]
        public long? NumeroInterior { get; set; }
        [Column("numeroExterior", TypeName = "INTEGER (5)")]
        public long NumeroExterior { get; set; }
        [Column("codigoPostal", TypeName = "INTEGER (5)")]
        public long CodigoPostal { get; set; }
        [Required]
        [Column("colonia", TypeName = "VARCHAR (30)")]
        public string Colonia { get; set; }
        [Required]
        [Column("municipio ", TypeName = "VARCHAR (30)")]
        public string Municipio { get; set; }
        [Required]
        [Column("fechaCumpleanos", TypeName = "DATE")]
        public byte[] FechaCumpleanos { get; set; }
        [Required]
        [Column("fechaIngreso", TypeName = "DATE")]
        public byte[] FechaIngreso { get; set; }
        [Column("areaTrabajo")]
        public long AreaTrabajo { get; set; }
        [Column("tipoEmpleado")]
        public long TipoEmpleado { get; set; }
        [Column("plantel")]
        public long Plantel { get; set; }

        [ForeignKey(nameof(AreaTrabajo))]
        [InverseProperty("Empleados")]
        public virtual AreaTrabajo AreaTrabajoNavigation { get; set; }
        [ForeignKey(nameof(Plantel))]
        [InverseProperty("Empleados")]
        public virtual Plantel PlantelNavigation { get; set; }
        [ForeignKey(nameof(TipoEmpleado))]
        [InverseProperty("Empleados")]
        public virtual TipoEmpleado TipoEmpleadoNavigation { get; set; }
        [InverseProperty(nameof(Login.UsuarioNavigation))]
        public virtual ICollection<Login> Logins { get; set; }
        [InverseProperty(nameof(Permiso.EmpleadoNavigation))]
        public virtual ICollection<Permiso> Permisos { get; set; }
    }
}
