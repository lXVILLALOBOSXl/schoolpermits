using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PermisosEntitiesLib
{
    [Table("Login")]
    public partial class Login
    {
        [Key]
        [Column("idLogin")]
        public long IdLogin { get; set; }
        [Required]
        [Column("password", TypeName = "VARCHAR (15)")]
        public string Password { get; set; }
        [Column("usuario")]
        public long Usuario { get; set; }

        [ForeignKey(nameof(Usuario))]
        [InverseProperty(nameof(Empleado.Logins))]
        public virtual Empleado UsuarioNavigation { get; set; }
    }
}
