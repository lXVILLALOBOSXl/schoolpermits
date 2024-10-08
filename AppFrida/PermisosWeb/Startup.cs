using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PermisosEntitiesLib;  // Referencia al namespace que contiene el contexto `Permisos`

namespace PermisosWeb
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            string dbPath = Path.Combine("../../DataBase", "Permisos.db");
            
            // Usar el contexto `Permisos` que está en `PermisosEntitiesLib`
            services.AddDbContext<Permisos>(options =>
                options.UseSqlite($"Data Source={dbPath}")
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapFallbackToPage("/Index");  // Asegúrate de que la página "Index.cshtml" esté presente
            });
        }
    }
}
