using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using tl2_proyecto_2024_Olme2.Models;
public class TableroController : Controller{

    private readonly ILogger<TableroController> _logger;
    private ITableroRepository repositorioTablero;
    //Agregado para operaciones como buscar los nombres de los usuarios para mostrarlos en las tareas donde fueron asignados.
    private IUsuariosRepository repositorioUsuarios; 
    //Agregado para operaciones como mostrar las tareas en VerTablero().
    private ITareasRepository repositorioTareas; 

    public TableroController(ILogger<TableroController> logger, ITableroRepository ReposiotrioTablero, IUsuariosRepository RepositorioUsuarios, ITareasRepository RepositorioTareas){
        _logger = logger;
        repositorioTablero = ReposiotrioTablero;
        repositorioUsuarios = RepositorioUsuarios;
        repositorioTareas = RepositorioTareas;
    }
    
    //Vista principal, si el usuario es admin se muestran todos los tableros, sino se muestran los tableros creados por ellos primero y luego donde hay tareas asignadas.
    public IActionResult Index(){ 
        try{
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login"); 

            var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 
            ViewData["EsAdmin"] = EsAdmin; 

            var tablerosVM = new List<ListarTablerosVM>();
            //Verificacion de admin, para ver si se manda todos los tableros a la lista, o solo los creados y con tareas asignadas.
            if(EsAdmin){

                var tableros = repositorioTablero.ListarTableros(); 
                tablerosVM = tableros.Select(t => new ListarTablerosVM(t)).ToList();
            
            }else{

                int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id")); 

                //Obtenemos solo los tableros con tareas asignadas para cierto usuario.
                var tableros = repositorioTablero.ListarTablerosConTareasAsignadas(idUsuario); 
                tablerosVM = tableros.Select(t => new ListarTablerosVM(t)).ToList();
                //Obtenemos solo los tableros creados por cierto usuario.
                var tablerosUsuario = repositorioTablero.ListarTablerosDeUsuario(idUsuario); 
                var tablerosUsuarioVM = tablerosUsuario.Select(t => new ListarTablerosVM(t)).ToList();
                //Se envia por ViewData ya que los otros tableros los mandamos por vista.
                ViewData["tablerosUsuario"] = tablerosUsuarioVM; 
            
            }
            
            return View(tablerosVM);
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "no cargaron correctamente los tableros."});

        }
    }    

    //Vista detallada de un tablero en especifico. Si es admin puede acceder, sino solo se accede si sos dueño o tenes tareas asignadas.
    public IActionResult VerTablero(int id){ 
        try{
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login"); 

            var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 

            //Se crea una variable llamada "origen" para guardar la url anterior, ya que luego si accedemos a una tarea, se puede acceder desde dos lugares distintos.
            HttpContext.Session.SetString("origen", "Tablero");

            var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
            
            var tareas = repositorioTareas.ListarTareasDeTablero(id);
            //Detalle estético, uso esto para mostrar el nombre del usuario a quien se le asigno la tarea (y sino pongo sin asignar).
            var tareasVM = tareas.Select(t =>{
                var nombreUsuarioAsignado = "Sin Asignar";
                
                if(t.idUsuarioAsignado!=-1 && t.idUsuarioAsignado!=null){
                    nombreUsuarioAsignado = repositorioUsuarios.ObtenerDetallesDeUsuario((int)t.idUsuarioAsignado).nombreDeUsuario;                
                }
                
                return new ListarTareasVM(t, nombreUsuarioAsignado);
            }).ToList();

            var usuarioPropietario = repositorioUsuarios.ObtenerDetallesDeUsuario(tablero.idUsuarioPropietario);

            //Si es admin se obtienen todos los datos del tablero, de todas las tareas y el nombre del dueño.
            var tableroVM = new VerTableroVM(tablero, tareasVM, usuarioPropietario.nombreDeUsuario); 
            //Verificamos si es admin para agregar acciones a todas las tareas o a solo las asignadas.
            if(!EsAdmin){ 

                int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
                
                var tablerosConTareasAsignadas = repositorioTablero.ListarTablerosConTareasAsignadas(idUsuario);

                var esDueño = tablero.idUsuarioPropietario == idUsuario;
                var tieneTareas = tablerosConTareasAsignadas.Any(t => t.id == id);
                //Se verifica si, el usuario que no es admin, cumple con los requisitos para ver dicho tablero (Ser dueño o tener tareas asignadas).
                if(!esDueño && !tieneTareas){ 

                    TempData["ErrorMessage"] = "Acceso denegado.";
                    //Sino se lo redirecciona al index de tablero, con un mensaje de acceso denegado.
                    return RedirectToAction("Index"); 
                
                }
                //Si cumple con los requisitos, se le agrega al VM ademas el id del usuario que controla el sistema, y un bool que nos dice si es dueño o no del tablero para establecer acciones para tareas.
                tableroVM = new VerTableroVM(tableroVM, esDueño, idUsuario); 
            
            }
            
            return View(tableroVM);
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "el tablero solicitado no existe."});

        }
    }

    [HttpGet]
    //Vista para crear un nuevo tablero, acceso libre.
    public IActionResult AltaTablero(){ 
        try{    
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            
            var model = new CrearTableroVM();
            
            var idUsuarioPropietario = HttpContext.Session.GetInt32("Id");
            //Se hace verificacion porque dice que puede ser null, pero si el usuario esta logueado no deberia. Se crea un modelo con el id del usuario que controla el sistema para que se coloque como propietario en la vista en un input hidden.
            if(idUsuarioPropietario.HasValue){
                model = new CrearTableroVM((int)idUsuarioPropietario); 
            }
            
            return View(model);
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");

        }
    }

    [HttpPost]
    //Método que crea un nuevo tablero.
    public IActionResult CrearTablero(CrearTableroVM tableroVM){
        try{
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) return RedirectToAction("AltaTablero");
            
            var tablero = new Tablero(tableroVM);
            repositorioTablero.CrearTablero(tablero);
            
            TempData["SuccessMessage"] = "¡Tablero \""+tableroVM.nombre+"\" creado con éxito!";
            return RedirectToAction("Index");
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return BadRequest("Creación de tablero sin éxito.");

        }
    }

    [HttpGet]
    //Vista para modificar un tablero dependiendo el id, solo se puede acceder a el si sos dueño del tablero o sos admin.
    public IActionResult ModificarTablero(int id){ 
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
            ViewData["EsAdmin"] = EsAdmin; 
            
            var usuarios = repositorioUsuarios.ListarUsuarios();
            var usuariosVM = usuarios.Select(u => new ListarUsuariosVM(u)).ToList();
            
            var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
            var tableroVM = new ModificarTableroVM(tablero, usuariosVM);
            
            //Verificamos si es admin para luego determinar si puede acceder el usuario o no.
            if(!EsAdmin){ 

                int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
                var esDueño = tablero.idUsuarioPropietario == idUsuario;
                //Se verifica si, el usuario que no es admin, es dueño del tablero al menos.
                if(!esDueño){ 
                    TempData["ErrorMessage"] = "Acceso denegado.";
                    //Sino se lo redirecciona al index de tablero, con un mensaje de acceso denegado.
                    return RedirectToAction("Index", "Tablero"); 
                }
            
            }

            return View(tableroVM);
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "el tablero solicitado no existe."});

        }
    }

    [HttpPost]
    //Método que modifica el tablero.
    public IActionResult Modificar(ModificarTableroVM tableroVM){ 
        try{
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) return RedirectToAction("ModificarTablero", tableroVM.id);

            var tablero = new Tablero(tableroVM);
            repositorioTablero.ModificarTablero(tablero.id, tablero);
            
            TempData["SuccessMessage"] = "¡Tablero \""+tableroVM.nombre+"\" modificado con éxito!";
            return RedirectToAction("Index");

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return BadRequest("Modificación de tablero sin éxito.");

        }
    }

    [HttpGet]
    //Vista para eliminar un tablero por id. Solo puede acceder el admin o el dueño del tablero y solo si el tablero no tiene tareas cargadas.
    public IActionResult EliminarTablero(int id){ 
        try{
        
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);

            var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 
            //Verificamos si es admin para luego determinar si puede acceder el usuario o no.
            if(!EsAdmin){

                int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
                var esDueño = tablero.idUsuarioPropietario == idUsuario;
                //Se verifica si, el usuario que no es admin, es dueño del tablero al menos.
                if(!esDueño){ 
                    TempData["ErrorMessage"] = "Acceso denegado.";
                    //Sino se lo redirecciona al index de tablero, con un mensaje de acceso denegado.
                    return RedirectToAction("Index", "Tablero"); 
                }

            }
            
            var tareas = repositorioTareas.ListarTareasDeTablero(id);
            //Verificamos que el tablero no tenga tareas asignadas. Si las tiene, solo se muestra un mensaje de alerta y no deja borrar el tablero.
            if(tareas.Count>0){ 
                ViewData["ErrorMessage"] = "No se puede borrar el tablero porque tiene tareas cargadas.";
            }
            
            var tableroVM = new EliminarTableroVM(tablero);
            return View(tableroVM);

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "el tablero solicitado no existe"});

        }
    }

    [HttpPost]
    //Metodo para eliminar un tablero por id.
    public IActionResult Eliminar(EliminarTableroVM tableroVM){ 
        try{    

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) return RedirectToAction("EliminarTablero", tableroVM.id);

            var tablero = new Tablero(tableroVM);
            repositorioTablero.EliminarTableroPorId(tablero.id);
            
            TempData["SuccessMessage"] = "¡Tablero \""+tableroVM.nombre+"\" eliminado con éxito!";
            return RedirectToAction("Index");
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return BadRequest("Eliminación de tablero sin éxito.");

        }
    }
}