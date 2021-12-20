using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using PermisosEntitiesLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Globalization;

namespace PermisosWeb.Pages
{
    /// <summary>
    /// Clase para modelar los permisos de la pagina
    /// </summary>
    public class PermisosModel : PageModel
    {
        [BindProperty]
        //Guarda la informacion que proviene del html Permiso
        public Permiso Permiso { get; set; }
        //Guarda los resultados de la query como propiedades no anonimas
        public List<PermisosHandler> listPermisos { get; set; }
        //Objeto para hacer la conexion y consultas con la base de datos
        private Permisos db;

        //Las siguientes propiedades guarda informacion de los permisos que se muestran, agregan y cancelan 

        public string Nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string Area { get; set; }
        public long idArea { get; set; }
        public string nombreCompleto { get; set; }
        public long Nomina { get; set; }
        public long tipoEmpleado { get; set; }
        public string Fecha { get; set; }
        public string fechaHtml { get; set; }
        public long Estado { get; set; }

        /// <summary>
        /// Hace la conexion con la base de datos
        /// </summary>
        /// <param name="injectedContext">Se pasa la ubicacion del archivo donde se encuentra el .sql</param>
        public PermisosModel(Permisos injectedContext)
        {
            db = injectedContext;
        }

        /// <summary>
        /// Guarda la infromacion de la base de datos y la muestra en el html
        /// </summary>
        public void OnGet()
        {
            //Guarda la query donde se consulta la informacion del empleado
            var queryEmpleado =
            (
                from e in db.Empleados
                join a in db.AreaTrabajos
                on e.AreaTrabajo equals a.IdAreaEmpleado
                where e.NumeroDeNomina == IndexModel.Nomina
                select new
                {
                    Nombre = e.Nombres,
                    apellidoPaterno = e.ApellidoPaterno,
                    apellidoMaterno = e.ApellidoMaterno,
                    Area = a.DescripcionArea,
                    idArea = a.IdAreaEmpleado
                }
            ).ToList();//Casteamos el resultado de la query en una lista de tipos anonimos

            //Asignamos valores a las propiedades de la clase PermisosModelcon el resultado de la queryEmpleado y se muestra en el html
            tipoEmpleado = IndexModel.tipoEmpleado;
            Nombre = queryEmpleado[0].Nombre;
            apellidoPaterno = queryEmpleado[0].apellidoPaterno;
            apellidoMaterno = queryEmpleado[0].apellidoMaterno;
            Area = queryEmpleado[0].Area;
            nombreCompleto = Nombre + " " + apellidoPaterno + " " + apellidoMaterno;
            Nomina = IndexModel.Nomina;
            idArea = queryEmpleado[0].idArea;
            Fecha = DateTime.Now.ToString("dd/MM/yyyy");
            fechaHtml = DateTime.Now.ToString("yyyy-MM-dd");

            //Dependiendo del tipo de empleado es la query que se realiza para traer la infromacion de los permisos
            switch (tipoEmpleado)
            {
                //En caso de que sea la directora
                case 1:
                    //Guarda la query donde se consultan los permisos propios y los permisos aceptados por el supervisor
                    var queryPermisosDir =
                    (
                        from p in db.Permiso
                        join tp in db.TipoPermisos
                        on p.TipoPermiso equals tp.IdTipoPermiso
                        join ep in db.EstadoPermisos
                        on p.EstadoPermiso equals ep.IdEstadoPermiso
                        join emp in db.Empleados
                        on p.Empleado equals emp.NumeroDeNomina
                        where (p.EstadoPermiso == 4 || (emp.NumeroDeNomina == IndexModel.Nomina))
                        select new
                        {
                            Folio = p.Folio,
                            Tipo = tp.DescripcionPermiso,
                            FechaElab = p.FechaElaboracion,
                            FechaInicio = p.FechaJustificacionInicio,
                            FechaFin = p.FechaJustificacionFin,
                            HoraInicio = p.HoraInicio,
                            HoraFin = p.HoraFin,
                            Estado = ep.DescripcionEstado,
                            Nomina = emp.NumeroDeNomina
                        }
                    ).ToList();//Casteamos el resultado de la query en una lista de tipos anonimos

                    //Dado que la query regresa una lista con propiedades anonimas no se puede asignar los valores a las propiedades de la clase directamente, fue necesario crear una clase auxiliar que mediante un ciclo permite guardar las propiedades, dejando de ser anonimas para despues asignarla a la propiedad lista de la clase Model
                    List<PermisosHandler> listaPermisosDir = new List<PermisosHandler>();

                    //Guarda las propiedades de la lista de la consulta en las propiedades de una lista con la clase auxiliar.
                    foreach (var item in queryPermisosDir)
                    {
                        var p = new PermisosHandler()
                        {
                            Folio = item.Folio.ToString(),
                            Tipo = item.Tipo,
                            FechaElab = item.FechaElab,
                            FechaInicio = item.FechaInicio,
                            FechaFin = item.FechaFin,
                            HoraInicio = item.HoraInicio,
                            HoraFin = item.HoraFin,
                            Estado = item.Estado,
                            Nomina = item.Nomina.ToString()
                        };
                        listaPermisosDir.Add(p);
                    }
                    //Una vez que la lista ya esta llena de objetos tipo permiso se iguala a una lista de la clase model
                    listPermisos = listaPermisosDir;
                    break;
                //caso supervisor profesor
                case 2:
                //Caso supervisor
                case 3:
                    //Guarda el resultadode la query donde se consultan los permisos propios y los permisos solicitados por los empleados de los que esta a cargo
                    var queryPermisosSup =
                   (
                       from p in db.Permiso
                       join tp in db.TipoPermisos
                       on p.TipoPermiso equals tp.IdTipoPermiso
                       join ep in db.EstadoPermisos
                       on p.EstadoPermiso equals ep.IdEstadoPermiso
                       join emp in db.Empleados
                       on p.Empleado equals emp.NumeroDeNomina
                       where ((p.EstadoPermiso == 3 && emp.AreaTrabajo == idArea) || (emp.NumeroDeNomina == IndexModel.Nomina))
                       select new
                       {
                           Folio = p.Folio,
                           Tipo = tp.DescripcionPermiso,
                           FechaElab = p.FechaElaboracion,
                           FechaInicio = p.FechaJustificacionInicio,
                           FechaFin = p.FechaJustificacionFin,
                           HoraInicio = p.HoraInicio,
                           HoraFin = p.HoraFin,
                           Estado = ep.DescripcionEstado,
                           Nomina = emp.NumeroDeNomina
                       }
                   ).ToList();//Casteamos el resultado de la query en una lista de tipos anonimos
                    //Guarda los resultados de la query
                    List<PermisosHandler> listaPermisosSup = new List<PermisosHandler>();
                    //Guarda las propiedades de la lista de la consulta en las propiedades de una lista con la clase auxiliar.
                    foreach (var item in queryPermisosSup)
                    {
                        var p = new PermisosHandler()
                        {
                            Folio = item.Folio.ToString(),
                            Tipo = item.Tipo,
                            FechaElab = item.FechaElab,
                            FechaInicio = item.FechaInicio,
                            FechaFin = item.FechaFin,
                            HoraInicio = item.HoraInicio,
                            HoraFin = item.HoraFin,
                            Estado = item.Estado,
                            Nomina = item.Nomina.ToString()
                        };
                        listaPermisosSup.Add(p);
                    }
                    //Una vez que la lista ya esta llena de objetos tipo permiso se iguala a una lista de la clase model
                    listPermisos = listaPermisosSup;
                    break;
                //Caso empleado
                case 4:
                    //Guarda el resultadod e la query donde se consultan los permisos propios 
                    var queryPermisos =
                    (
                        from p in db.Permiso
                        join tp in db.TipoPermisos
                        on p.TipoPermiso equals tp.IdTipoPermiso
                        join ep in db.EstadoPermisos
                        on p.EstadoPermiso equals ep.IdEstadoPermiso
                        where p.Empleado == IndexModel.Nomina
                        select new
                        {
                            Folio = p.Folio,
                            Tipo = tp.DescripcionPermiso,
                            FechaElab = p.FechaElaboracion,
                            FechaInicio = p.FechaJustificacionInicio,
                            FechaFin = p.FechaJustificacionFin,
                            HoraInicio = p.HoraInicio,
                            HoraFin = p.HoraFin,
                            Estado = ep.DescripcionEstado
                        }
                    ).ToList();
                    //Guarda los resultados de la query
                    List<PermisosHandler> listaPermisos = new List<PermisosHandler>();
                    //Guarda las propiedades de la lista de la consulta en las propiedades de una lista con la clase auxiliar.
                    foreach (var item in queryPermisos)
                    {
                        var p = new PermisosHandler()
                        {
                            Folio = item.Folio.ToString(),
                            Tipo = item.Tipo,
                            FechaElab = item.FechaElab,
                            FechaInicio = item.FechaInicio,
                            FechaFin = item.FechaFin,
                            HoraInicio = item.HoraInicio,
                            HoraFin = item.HoraFin,
                            Estado = item.Estado
                        };
                        listaPermisos.Add(p);
                    }
                    //Una vez que la lista ya esta llena de objetos tipo permiso se iguala a una lista de la clase model
                    listPermisos = listaPermisos;
                    break;
            }

        }

        /// <summary>
        /// Listener para enviar un permiso
        /// </summary>
        /// <returns>La misma pagina pero actualizada con la informacion de los permisos</returns>
        public IActionResult OnPostEnviarPermiso()
        {
            //Checa si el permiso es valido antes de gnerarlo
            if (isValid())
            {
                //Si la hora de inicio en el permiso de 2 horas fue a las 7:00
                if (Permiso.HoraInicio == "7:00")
                {
                    //la hora final sera las 9:00
                    Permiso.HoraFin = "9:00";
                }
                //Si la hora de inicio en el permiso de 2 horas fue a las 13:00
                else if (Permiso.HoraInicio == "13:00")
                {
                    //la hora final sera las 15:00
                    Permiso.HoraFin = "15:00";
                }
                else //si no se tiene hora de inicio no es permiso de 2 horas 
                {
                    //entonces el valor sera nulo
                    Permiso.HoraFin = null;
                }

                //Asignamos a la propiedad de la fecha de justificacion inicio el dato que se recibe del html
                Permiso.FechaJustificacionInicio = DateTime.Parse(Permiso.FechaJustificacionInicio).ToString("dd/MM/yyyy");
                //Asignamos a la propiedad de la fecha de justificacion fin el dato que se recibe del html
                Permiso.FechaJustificacionFin = DateTime.Parse(Permiso.FechaJustificacionFin).ToString("dd/MM/yyyy");
                //Asignamos a la propiedad de la fecha de elaboracion el dia en el que se esta agregando el permiso
                Permiso.FechaElaboracion = DateTime.Now.ToString("dd/MM/yyyy");
                //agregamos la propiedad de nomina, la misma nomina con la cual se hizo login en el index
                Permiso.Empleado = IndexModel.Nomina;
                if (IndexModel.tipoEmpleado == 1) //Si el que agrego un permiso fue la directora
                {
                    Permiso.EstadoPermiso = 4; //El estado sera aprobado por supervisor ya que ella no tiene supervisor, solo falta que ella acepte su propio permiso
                }
                else //de cualquier otro modo (empleado y supervisor)
                {
                    Permiso.EstadoPermiso = 3; //El estado sera solicitado por que debe ser aceptado por el supervisor y la directora
                }
                //a la base de datos se agrega un nuevo permiso
                db.Permiso.Add(Permiso);
                //se guardan los cambios
                db.SaveChanges();
                //carga la misma pagina actualizada con los permnisos
                return RedirectToPage("/permisos");
            }
            //Si no es valido
            else
            {
                //Alerta al usuario que su permiso es invalido
                TempData["PermisoMessage"] = "No se pudo crear el permiso, consulte las condiciones e intentelo de nuevo";
                //Lo redirige a la pagina
                return RedirectToPage("/permisos");
            }
        }

        /// <summary>
        /// Determina mediante condiciones todas las restricciones necesarias para generar un permiso
        /// </summary>
        /// <returns>Si el permiso es valido, de acuerdo a las condiciones retorna un true, si no un false</returns>
        public bool isValid()
        {
            //Almacena el dia de la semana en el que se solicita el permiso para verificar si no se hizo en fin de semana
            DayOfWeek fechaValidacionInicio = DateTime.Parse(Permiso.FechaJustificacionInicio).DayOfWeek;
            DayOfWeek fechaValidacionFin = DateTime.Parse(Permiso.FechaJustificacionFin).DayOfWeek;
            //Almacena las fechas de justificaciones de inicio y fin en una instancia de la clase datetime para no tener que utilizar el objeto de la clase Permiso con propiedad string, ya que funciones de datetime son requeridas para las validaciones
            DateTime dateInicio = DateTime.Parse(Permiso.FechaJustificacionInicio);
            DateTime dateFin = DateTime.Parse(Permiso.FechaJustificacionFin);
            //Almacenan el numero de permisos de dos horas que existen en las 2 diferentes quincenas de cada mes
            int primeraQuincena = 0, segundaQuincena = 0;
            //Si en alguna de las fechas a justificar el permiso esta en fin de semana

            if ((fechaValidacionInicio == DayOfWeek.Saturday) || (fechaValidacionInicio == DayOfWeek.Sunday) || (fechaValidacionFin == DayOfWeek.Saturday) || (fechaValidacionFin == DayOfWeek.Sunday))
            {


                //El permiso es invalido
                return false;
            }

            var queryPermisoValido =
            (
                from p in db.Permiso
                join tp in db.TipoPermisos
                on p.TipoPermiso equals tp.IdTipoPermiso
                join ep in db.EstadoPermisos
                on p.EstadoPermiso equals ep.IdEstadoPermiso
                where p.Empleado == IndexModel.Nomina
                where p.FechaJustificacionInicio == dateInicio.ToString("dd/MM/yyyy")
                select new
                {
                    Folio = p.Folio,
                }
            ).ToList();
            if (queryPermisoValido.Count != 0)
            {

                return false;
            }

            //Si el permiso es de dos horas o cumpleaños
            else if (Permiso.TipoPermiso == 2 || Permiso.TipoPermiso == 3)
            {
                //Estos tipos de permisos solo tienen una duracion de 1 dia asi que si la fecha de justificacion no es la misma
                if (Permiso.FechaJustificacionInicio != Permiso.FechaJustificacionFin)
                {
                    //quiere decir que abarca mas de un dia y por lo tanto el permiso es invalido
                    return false;
                }
                else if (Permiso.TipoPermiso == 3) //Si el permiso es de 2 horas y esta correcto el dia a justificar
                {
                    //Se tiene que analizar que no sean mas de 2 permisos por semana
                    //Guardamos en un string auxiliar la fecha de justificacion con el formato que acepta DateTime para hacer parsse despues
                    string auxFechaFin = dateInicio.ToString("yyyy-MM-dd");
                    //Convertimos el string en un arreglo de chars para modificar el dia y poder comparalo para saber en que quincena se encuentra del ems, pero el mes y el a;o sigue siendo el mismo que la fecha de justificacion
                    char[] quincena = auxFechaFin.ToCharArray(0, 10);
                    //Cambiamos el dia por el 15
                    quincena[8] = '1';
                    quincena[9] = '5';
                    //Convertimos el arreglo de char nuevamente en strings
                    auxFechaFin = new string(quincena);
                    //Convertimos el objeto string en DateTime para poder hacer una comparacion y determinar en que quincena se encuentra
                    DateTime fechaFinAux = DateTime.Parse(auxFechaFin);
                    //Si mi fecha de justificacion es mayor al dia 15 de ese mismo mes y a;o quiere decir que esta en la segyunda quincena
                    if (dateInicio > fechaFinAux)
                    {
                        //Segunda quincena
                        //Recorremos todos los dias de la segunda quincena, desde el 16 hasta el ultimo dia del mes
                        for (int i = 16; i <= DateTime.DaysInMonth(dateInicio.Year, dateInicio.Month); i++)
                        {
                            //pasamos el contador a un string para poder hacer parse y cambiar de formato en la fecha para poder hacer una consulta correcta
                            string day = i.ToString();
                            quincena[8] = day[0];
                            quincena[9] = day[1];
                            auxFechaFin = new string(quincena);
                            auxFechaFin = DateTime.Parse(auxFechaFin).ToString("dd/MM/yyyy");
                            //hacemos una query en cada dia de la segunda quincena para determinar si ya existen permisos en dicha quincena
                            var queryPermisos =
                            (
                                from p in db.Permiso
                                join tp in db.TipoPermisos
                                on p.TipoPermiso equals tp.IdTipoPermiso
                                join ep in db.EstadoPermisos
                                on p.EstadoPermiso equals ep.IdEstadoPermiso
                                where p.Empleado == IndexModel.Nomina
                                where p.TipoPermiso == 3
                                where p.FechaJustificacionInicio == auxFechaFin
                                select new
                                {
                                    Folio = p.Folio,
                                }
                            ).ToList();
                            //Si encontro un permiso
                            if (queryPermisos.Count > 0)
                            {
                                //aumenta el contador
                                segundaQuincena++;
                            }
                        }
                        //Si ya que termino existir=eron mas de 2 permisos
                        if (segundaQuincena > 1)
                        {
                            //El permiso queda invalido
                            return false;
                        }
                    }
                    else
                    {
                        //Primera quincena
                        for (int i = 1; i < 16; i++)
                        {
                            string day = i.ToString();
                            //Esta condicion es para los dias donde en el formato de la fecha se antepone un 0
                            if (i > 0 && i < 10)
                            {
                                quincena[8] = '0';
                                quincena[9] = day[0];
                                auxFechaFin = new string(quincena);
                            }
                            else
                            {
                                quincena[8] = day[0];
                                quincena[9] = day[1];
                                auxFechaFin = new string(quincena);
                            }
                            auxFechaFin = DateTime.Parse(auxFechaFin).ToString("dd/MM/yyyy");
                            var queryPermisos =
                            (
                                from p in db.Permiso
                                join tp in db.TipoPermisos
                                on p.TipoPermiso equals tp.IdTipoPermiso
                                join ep in db.EstadoPermisos
                                on p.EstadoPermiso equals ep.IdEstadoPermiso
                                where p.Empleado == IndexModel.Nomina
                                where p.TipoPermiso == 3
                                where p.FechaJustificacionInicio == auxFechaFin
                                select new
                                {
                                    Folio = p.Folio,
                                }
                            ).ToList();
                            if (queryPermisos.Count > 0)
                            {
                                primeraQuincena++;
                            }
                        }
                        if (primeraQuincena > 1)
                        {
                            return false;
                        }
                    }
                }else if (Permiso.TipoPermiso == 2) //Si el permiso es de cumpleaños y esta correcto el dia a justificar
                {
                    var queryPermisos =
                        (
                            from p in db.Permiso
                            join tp in db.TipoPermisos
                            on p.TipoPermiso equals tp.IdTipoPermiso
                            join ep in db.EstadoPermisos
                            on p.EstadoPermiso equals ep.IdEstadoPermiso
                            where p.Empleado == IndexModel.Nomina
                            where p.TipoPermiso == 2

                            select new
                            {
                                Folio = p.Folio,
                            }
                        ).ToList();
                        //Si encontro un permiso
                        if (queryPermisos.Count > 0)
                        {
                            //No puede agregar otro y se regresa
                            return false;
                        }
                }
            }
            //Si el tipo de permiso es economico 
            else if (Permiso.TipoPermiso == 1)
            {
                //Se determina que la duracion de este no sea mayor de 3
                if ((dateFin - dateInicio).TotalDays > 2)
                {
                    //Si es mayor de 3 es un permiso invalido
                    return false;
                }
                string auxFechaFin = dateInicio.ToString("dd/MM/yyyy");
                //Convertimos el string en un arreglo de chars para modificar el dia y poder comparalo para saber en que quincena se encuentra del ems, pero el mes y el a;o sigue siendo el mismo que la fecha de justificacion
                char[] mes = auxFechaFin.ToCharArray(0, 10);

                //Iteramos desde el principio al ultimo dia del mes para hacer una consulta que nos arroje cuantos permisos econocmicos hay en el mes que se quiere hacer un permiso
                for (int i = 1; i <= DateTime.DaysInMonth(dateInicio.Year, dateInicio.Month); i++)
                {
                    //pasamos el contador a un string para poder hacer parse y cambiar de formato en la fecha para poder hacer una consulta correcta
                    string day = i.ToString();
                    if (i > 0 && i < 10)
                    {
                        mes[0] = '0';
                        mes[1] = day[0];
                        auxFechaFin = new string(mes);
                    }
                    else
                    {
                        mes[0] = day[0];
                        mes[1] = day[1];
                        auxFechaFin = new string(mes);
                    }
                    var queryPermisosMes =
                    (
                        from p in db.Permiso
                        join tp in db.TipoPermisos
                        on p.TipoPermiso equals tp.IdTipoPermiso
                        join ep in db.EstadoPermisos
                        on p.EstadoPermiso equals ep.IdEstadoPermiso
                        where p.Empleado == IndexModel.Nomina
                        where p.TipoPermiso == 1
                        where p.FechaJustificacionInicio == auxFechaFin
                        select new
                        {
                            Folio = p.Folio,
                        }
                    ).ToList();
                    if (queryPermisosMes.Count > 0)
                    {
                        //Si ya hay uno en el mes ya no puede generar otro
                        return false;
                    }
                }

                //por ultimo hacemos una consulta pra validar si exiten menos de 9 permisos para poder crear otro
                var queryPermisos =
                (
                    from p in db.Permiso
                    join tp in db.TipoPermisos
                    on p.TipoPermiso equals tp.IdTipoPermiso
                    join ep in db.EstadoPermisos
                    on p.EstadoPermiso equals ep.IdEstadoPermiso
                    where p.Empleado == IndexModel.Nomina
                    where p.TipoPermiso == 1

                    select new
                    {
                        Folio = p.Folio,
                    }
                ).ToList();
                //Si encontro un permiso
                if (queryPermisos.Count > 8)
                {
                    //Si ya hay 9 no se puede generar otro
                    return false;
                }
            }
            //Si llega aqui es por que todas las condiciones se cumplieron entonces puede generar el permiso
            return true;
        }

        /// <summary>
        /// Listener para cancelar un permiso desde empleado
        /// </summary>
        /// <param name="folio">Recibe el folio desde el html para hacer la consulta de folio cuyo permiso se va a eliminar</param>
        /// <returns>La misma pagina pero actualizada con la informacion de los permisos</returns>
        public IActionResult OnPostCancelarPermisoEmpleado(int folio)
        {
            //Guarda la consulta de la query que arroja el permiso que se quiere eliminar
            IEnumerable<Permiso> permisoDelete = db.Permiso.Where(pd => pd.Folio == folio);
            //Se elimina el permiso de la base de datos, porque si el empleado cancela mo hay nadie que deba aceptarlo
            db.RemoveRange(permisoDelete);
            //Se guardan los cambios y guardamos el numero de permisos eliminados
            int affectedRows = db.SaveChanges();
            //carga la misma pagina actualizada con los permnisos
            return RedirectToPage("/permisos");
        }

        /// <summary>
        /// Listener para cancelar un permiso
        /// </summary>
        /// <param name="folio">Recibe el folio desde el html para hacer la consulta de folio cuyo permiso se va a aceptar</param>
        /// <returns></returns>
        public IActionResult OnPostAceptarPermiso(int folio)
        {
            //si la persona que acepta el permiso es la directora
            if (IndexModel.tipoEmpleado == 1)
            {
                //Guardamos la query que arroja la informacion del permiso que se va aceptar
                Permiso aceptarPermiso = db.Permiso.First(pd => pd.Folio == folio);
                aceptarPermiso.EstadoPermiso = 1; //Cambia el estado del permiso a Aceptado por el director
                                                  //Se guardan los cambios
                int affected = db.SaveChanges();
                //carga la misma pagina actualizada con los permnisos
                return RedirectToPage("/permisos");
            }
            else //Si la persona por aceptar es el supervisor
            {
                //Guardamos la query que arroja la informacion del permiso que se va aceptar
                Permiso aceptarPermiso = db.Permiso.First(pd => pd.Folio == folio);
                aceptarPermiso.EstadoPermiso = 4;  //Cambia el estado del permiso a Aceptado por el supervisor
                //Se guardan los cambios
                int affected = db.SaveChanges();
                //carga la misma pagina actualizada con los permnisos
                return RedirectToPage("/permisos");
            }
        }

        /// <summary>
        /// Listener para cancelar un permiso desde supervisor o directora
        /// </summary>
        /// <param name="folio">Recibe el folio desde el html para hacer la consulta de folio cuyo permiso se va a cancelar</param>
        /// <returns></returns>
        public IActionResult OnPostCancelarPermiso(int folio)
        {

            //Guardamos la query que arroja la informacion del permiso que se va cancelar
            Permiso aceptarPermiso = db.Permiso.First(pd => pd.Folio == folio);
            //Si el supervisor o directora se van a cancelar su permiso, significa que se tiene que eliminar por que no hay nadie que lo tenga que aceptar
            if (aceptarPermiso.Empleado == IndexModel.Nomina)
            {
                //Guardamos la query que arroja la informacion del permiso que se va eliminar  
                IEnumerable<Permiso> permisoDelete = db.Permiso.Where(pd => pd.Folio == folio);
                //Se elimina de la base de datos
                db.RemoveRange(permisoDelete);
                //Se guardan los cambios en la base de datos
                int affectedRows = db.SaveChanges();
                //carga la misma pagina actualizada con los permnisos
                return RedirectToPage("/permisos");
            }
            //Si cancelan algun permiso que no es propio, el estado del permiso pasa a ser cancelado 
            aceptarPermiso.EstadoPermiso = 2;
            //Se guardan los cambios en la base de datos
            int affected = db.SaveChanges();
            //carga la misma pagina actualizada con los permnisos
            return RedirectToPage("/permisos");
        }

        /// <summary>
        /// Listener para boton salir
        /// </summary>
        /// <returns>Carga la pagina inicial</returns>
        public IActionResult OnPostSalir()
        {
            return RedirectToPage("/index");

        }
    }

    /// <summary>
    /// Clase auxiliar para guardar los objetos anonimos que resultan de las queryies y asi poder asignarlos a la clase de model
    /// </summary>
    public class PermisosHandler
    {

#nullable enable
        public string Folio { get; set; }

        public string Tipo { get; set; }
        public string FechaElab { get; set; }

        public string FechaInicio { get; set; }

        public string FechaFin { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFin { get; set; }

        public string Estado { get; set; }

        public string? Nomina { get; set; }

    }

}