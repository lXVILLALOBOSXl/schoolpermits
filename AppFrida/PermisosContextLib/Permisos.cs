using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PermisosEntitiesLib
{
    public partial class Permisos : DbContext
    {
        public Permisos()
        {
        }

        public Permisos(DbContextOptions<Permisos> options)
            : base(options)
        {
        }

        public virtual DbSet<AreaTrabajo> AreaTrabajos { get; set; }
        public virtual DbSet<Empleado> Empleados { get; set; }
        public virtual DbSet<EstadoPermiso> EstadoPermisos { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Permiso> Permiso { get; set; } /* i change permisos for permiso */
        public virtual DbSet<Plantel> Plantels { get; set; }
        public virtual DbSet<TipoEmpleado> TipoEmpleados { get; set; }
        public virtual DbSet<TipoPermiso> TipoPermisos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Filename= ../../DataBase/Permisos.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasOne(d => d.AreaTrabajoNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.AreaTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PlantelNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.Plantel)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TipoEmpleadoNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.TipoEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasOne(d => d.EmpleadoNavigation)
                    .WithMany(p => p.Permisos)
                    .HasForeignKey(d => d.Empleado)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EstadoPermisoNavigation)
                    .WithMany(p => p.Permisos)
                    .HasForeignKey(d => d.EstadoPermiso)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TipoPermisoNavigation)
                    .WithMany(p => p.Permisos)
                    .HasForeignKey(d => d.TipoPermiso)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
