using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using PermisosEntitiesLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace PermisosWeb.Pages
{
    /// <summary>
    /// Clase para modelar el login de la pagina
    /// </summary>
    public class IndexModel : PageModel
    {
        [BindProperty]
        //Se guarda la informacion que proviene del html login
        public Login Login { get; set; } 
        //Guarda el resultado de la query que consulta si la contraseña y el usuario son correctos
        public IQueryable<Login> isInDB { get; set; }
        //Guarda el tipo de empleado que hace login, es una propiedad estatica por que se pasa a la clase permiso cuando ingresa
        public static long tipoEmpleado { get; set; }
        //Guarda la nomina de empleado que hace login, es una propiedad estatica por que se pasa a la clase permiso cuando ingresa
        public static long Nomina { get; set; }
        // public dynamic ViewBag { get; }
        //Objeto para hacer la conexion y consultas con la base de datos
        private Permisos db;

        /// <summary>
        /// Hace la conexion con la base de datos
        /// </summary>
        /// <param name="injectedContext">Se pasa la ubicacion del archivo donde se encuentra el .sql</param>
        public IndexModel(Permisos injectedContext)
        {
            db = injectedContext;
        }

        /// <summary>
        /// Listener para el boton de ingresar del html de login
        /// </summary>
        /// <returns>Si la query de consulta de usuario y contraseña es correcta retorna la pagina para solicitar un permiso, si no vuelve a cragar el login</returns>
        public IActionResult OnPost()
        {
            //Query para consultar que el usuario y la contraseña ingresada exitan en la base de datos
            isInDB = db.Logins.Where(user => user.Usuario == Login.Usuario).Where(pass => pass.Password == Login.Password); 
            //Si existe
            if (isInDB.Any())
            {
                //A la propiedad de Nomina asignamos el valor de usuario del login
                Nomina = Login.Usuario;
                //Query para consultar el tipo de empleado que hace el login
                var tipoEmpleadoQuery =
                (
                    from t in db.Empleados
                    where t.NumeroDeNomina == IndexModel.Nomina
                    select new
                    {
                        TipoEmpleado = t.TipoEmpleado
                    }
                ).ToList(); //Casteamos el resultado de la query en una lista de tipos anonimos
                //La lista con el resultaado de la query en la posicion 0 contiene el tipo de empleado que hace el login y se asigna a la propiedad tipoempleado
                tipoEmpleado = tipoEmpleadoQuery[0].TipoEmpleado;
                //carga la pagina para solicitar un permiso
                return RedirectToPage("/permisos");
            }
            //Si el login no fue exitoso, carga la pagina de login
            TempData["AlertMessage"] = "Usuario o Password incorrectos";
            return Page();
        }
    }
}