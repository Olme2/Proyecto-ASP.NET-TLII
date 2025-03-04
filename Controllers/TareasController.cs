using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_proyecto_2024_Olme2.Models;
public class TareasController : Controller{

    private readonly ILogger<TareasController> _logger;
    private ITareasRepository repositorioTareas;
    //Agregado para operaciones como designar usuarios para tareas.
    private IUsuariosRepository repositorioUsuarios;
    //Agregado para operaciones como determinar si un usuario es dueño de cierto tablero donde se encuentra cierta tarea o no. 
    private ITableroRepository repositorioTablero; 

    public TareasController(ILogger<TareasController> logger, ITareasRepository RepositorioTareas, IUsuariosRepository RepositorioUsuarios, ITableroRepository RepositorioTablero){
        _logger = logger;
        repositorioTareas = RepositorioTareas;
        repositorioUsuarios = RepositorioUsuarios;
        repositorioTablero = RepositorioTablero;
    }
    
    //Vista principal de las tareas. Si el usuario es Admin muestra absolutamente todas las tareas, sino solo muestras a las que fueron asignadas. Si el usuario es dueño del tablero de dicha tarea (es decir, se la asigno a si mismo) la puede modificar y borrar.
    public IActionResult Index(){ 
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            //Seteamos una variable global llamada origen para luego implementarla en otros metodos que se puedan acceder desde dos vistas distintas.
            HttpContext.Session.SetString("origen", "Tareas");
            
            var tareas = new List<Tareas>();
            
            var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 
            ViewData["EsAdmin"] = EsAdmin;
            //Comprobamos si el usuario es admin para listar o no todas las tareas.
            if(EsAdmin){ 
                tareas = repositorioTareas.ListarTareas();
            }else{
                var id = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
                tareas = repositorioTareas.ListarTareasDeUsuario(id);
            }
            
            //Detalle estético, uso esto para mostrar el nombre del usuario a quien se le asigno la tarea (y sino pongo sin asignar).
            var tareasVM = tareas.Select(t =>{
                var nombreUsuarioAsignado = "Sin Asignar";
                if(t.idUsuarioAsignado!=-1 && t.idUsuarioAsignado!=null){
                    nombreUsuarioAsignado = repositorioUsuarios.ObtenerDetallesDeUsuario((int)t.idUsuarioAsignado).nombreDeUsuario;
                }
                return new ListarTareasVM(t, nombreUsuarioAsignado);
            }).ToList(); 
            
            return View(tareasVM);

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");

        }
    }

    [HttpGet]
    //Vista para crear una nueva tarea recibiendo el id de un tablero. Solo se puede acceder si es dueño del tablero o admin.
    public IActionResult AltaTarea(int id){ 
        try{    
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            var idDueño = tablero.idUsuarioPropietario;

            var EsDueño = idUsuario == idDueño;
            //Verificamos que el usuario sea dueño para poder acceder.
            if(!EsDueño){ 
                TempData["ErrorMessage"] = "Acceso denegado.";
                return RedirectToAction("VerTablero", "Tablero", new {id = tablero.id});
            }

            var tareaVM = new CrearTareaVM(id); 
            return View(tareaVM);

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");

        }
    }

    [HttpPost]
    //Método para crear una nueva tarea.
    public IActionResult CrearTarea(CrearTareaVM tareaVM){ 
        try{

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) RedirectToAction("AltaTarea");

            var tarea = new Tareas(tareaVM);
            repositorioTareas.CrearTarea(tarea.idTablero, tarea);

            TempData["SuccessMessage"] = "¡Tarea \""+tareaVM.nombre+"\" creada con éxito!";
            return RedirectToAction("VerTablero", "Tablero", new {id = tarea.idTablero});

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return BadRequest("Creación de tarea sin éxito.");
        
        }
    }

    [HttpGet]
    //Vista para modificar una determinada tarea. Se puede acceder a ella solo si sos admin, dueño del tablero donde se encuentra dicha tarea o si fuiste asignado para ella tarea, sino no. 
    public IActionResult ModificarTarea(int id){ 
        try{

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var origen = HttpContext.Session.GetString("origen");
            //Guardamos la anterior direccion para redireccionar despues en la vista a la vista anterior correcta si se requiere.
            TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();

            var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
            var tareaVM = new ModificarTareaVM(tarea);
            
            var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
            //Verificamos si es admin para dar control total a la modificacion sin importar si es o no dueño, si no es admin se verifica que al menos sea dueño o esté asignado para acceder.
            if(!EsAdmin){ 
                
                var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
                var idUsuarioAsignado = tarea.idUsuarioAsignado;
                var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.idTablero).idUsuarioPropietario;
                
                var EsDueño = idUsuario == idDueño;
                var EstaAsignado = idUsuario == idUsuarioAsignado;
                //Verificamos si el usuario no es ni dueño ni asignado, si no es ninguna de las dos no accede.
                if(!EsDueño && !EstaAsignado){ 
                    TempData["ErrorMessage"] = "Acceso denegado.";
                    //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede modificar desde el index de tareas o desde VerTablero() en el controller de tablero.
                    if(origen == "Tablero"){ 
                        return RedirectToAction("VerTablero", "Tablero", new {id = tarea.idTablero});
                    }else{
                        return RedirectToAction("Index");
                    }
                }

                tareaVM = new ModificarTareaVM(tareaVM, EsDueño);
            }
            return View(tareaVM);
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");

        }
    }

    [HttpPost]
    //Método para modificar la tarea
    public IActionResult Modificar(ModificarTareaVM tareaVM){ 
        try{
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) RedirectToAction("ModificarTarea", new {id = tareaVM.id});
            
            var origen = HttpContext.Session.GetString("origen");
            
            var tarea = new Tareas(tareaVM);
            repositorioTareas.ModificarTarea(tarea.id,tarea);

            TempData["SuccessMessage"] = "¡Tarea \""+tareaVM.nombre+"\" modificada con éxito!";
            if(origen == "Tablero"){ //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede modificar desde el index de tareas o desde VerTablero() en el controller de tablero.
                return RedirectToAction("VerTablero", "Tablero", new {id = tareaVM.idTablero});
            }
            
            return RedirectToAction("Index");
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return BadRequest("Modificación de tarea sin éxito.");

        }
    }

    [HttpGet]
    //Vista para asignar o reasignar un usuario a una determinada tarea. Se puede acceder a esta vista siendo admin o dueño del tablero donde se encuentra la tarea, sino no.
    public IActionResult AsignarUsuario(int id){
        try{ 

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var origen = HttpContext.Session.GetString("origen");
            //Guardamos la anterior direccion para redireccionar despues en la vista a la vista anterior correcta si se requiere.
            TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();

            var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);

            var listaDeUsuarios = repositorioUsuarios.ListarUsuarios();
            var listaDeUsuariosVM = listaDeUsuarios.Select(u => new ListarUsuariosVM(u)).ToList();

            var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
            TempData["EsAdmin"] = EsAdmin;
            //Verificamos si el usuario no es admin, si no lo es procedemos a verificar si al menos es dueño.
            if(!EsAdmin){ 
            
                var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
                var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.idTablero).idUsuarioPropietario;
                var esDueño = idUsuario == idDueño;
                //Si el usuario no es dueño se lo saca.
                if(!esDueño){ 
                    TempData["ErrorMessage"] = "Acceso denegado.";
                    //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede modificar desde el index de tareas o desde VerTablero() en el controller de tablero.
                    if(origen == "Tablero"){ 
                        return RedirectToAction("VerTablero", "Tablero", new {id = tarea.idTablero});
                    }else{
                        return RedirectToAction("Index");
                    }
                }

            }
            
            var model = new AsignarUsuarioVM(tarea, listaDeUsuariosVM);
            return View(model);
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");

        }
    }

    [HttpPost]
    //Método para asignar o reasignar un usuario a una determinada tarea.
    public IActionResult Asignar(AsignarUsuarioVM model){
        try{ 
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) return RedirectToAction ("AsignarUsuario", new{id = model.idTarea});
            
            var origen = HttpContext.Session.GetString("origen");
            
            var tarea = new Tareas(model);
            repositorioTareas.AsignarUsuarioATarea(tarea.idUsuarioAsignado, tarea.id);
            
            TempData["SuccessMessage"] = "¡Usuario asignado correctamente a la tarea \""+tarea.nombre+"\"!";
            //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede asignar desde el index de tareas o desde VerTablero() en el controller de tablero.
            if(origen == "Tablero"){ 
                return RedirectToAction("VerTablero", "Tablero", new {id = model.idTablero});
            }

            return RedirectToAction("Index");
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return BadRequest("Asignación de usuario sin éxito.");

        }
    }

    [HttpGet]
    //Vista para eliminar una determinada tarea. Se puede acceder a ella siendo admin o dueño del tablero, sino no.
    public IActionResult EliminarTarea(int id){ 
        try{

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var origen = HttpContext.Session.GetString("origen");
            //Guardamos la anterior direccion para redireccionar despues en la vista a la vista anterior correcta si se requiere.
            TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
            
            var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
            
            var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
            //Verificamos si el usuario es admin, sino que al menos sea dueño.
            if(!EsAdmin){ 

                var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
                var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.idTablero).idUsuarioPropietario;
                var esDueño = idUsuario == idDueño;
                //Si no es dueño se niega el acceso.
                if(!esDueño){ 
                    TempData["ErrorMessage"] = "Acceso denegado";
                    //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede eliminar desde el index de tareas o desde VerTablero() en el controller de tablero.
                    if(origen == "Tablero"){ 
                        return RedirectToAction("VerTablero", "Tablero", new {id=tarea.idTablero});
                    }else{
                        return RedirectToAction("Index");
                    }
                }
                
            }
            
            var tareaVM = new EliminarTareaVM(tarea);
            return View(tareaVM);
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");

        }
    }

    [HttpPost]
    //Método para eliminar una tarea determinada por id.
    public IActionResult Eliminar(EliminarTareaVM tareaVM){ 
        try{

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction("Index", "Login");
            if(!ModelState.IsValid) return RedirectToAction("EliminarTarea", new {id =tareaVM.id});

            var origen = HttpContext.Session.GetString("origen");

            var tarea = new Tareas(tareaVM);
            repositorioTareas.EliminarTarea(tarea.id);

            TempData["SuccessMessage"] = "¡Tarea \""+tareaVM.nombre+"\" eliminada con éxito!";
            //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede eliminar desde el index de tareas o desde VerTablero() en el controller de tablero.
            if(origen == "Tablero"){ 
                return RedirectToAction("VerTablero", "Tablero", new {id = tarea.idTablero});
            }
            
            return RedirectToAction("Index");
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return BadRequest("Eliminación de tarea sin éxito.");
        
        }
    }

}