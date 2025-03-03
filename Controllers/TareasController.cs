using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_proyecto_2024_Olme2.Models;
public class TareasController : Controller{
    private readonly ILogger<TareasController> _logger;
    private ITareasRepository repositorioTareas;
    private IUsuariosRepository repositorioUsuarios; //Agregado para operaciones como designar usuarios para tareas.
    private ITableroRepository repositorioTablero; //Agregado para operaciones como determinar si un usuario es dueño de cierto tablero donde se encuentra cierta tarea o no.
    public TareasController(ILogger<TareasController> logger, ITareasRepository RepositorioTareas, IUsuariosRepository RepositorioUsuarios, ITableroRepository RepositorioTablero){
        _logger = logger;
        repositorioTareas = RepositorioTareas;
        repositorioUsuarios = RepositorioUsuarios;
        repositorioTablero = RepositorioTablero;
    }
    public IActionResult Index(){ //Vista principal de las tareas. Si el usuario es Admin muestra absolutamente todas las tareas, sino solo muestras a las que fueron asignadas. Si el usuario es dueño del tablero de dicha tarea (es decir, se la asigno a si mismo) la puede modificar y borrar.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 
        ViewData["EsAdmin"] = EsAdmin;
        HttpContext.Session.SetString("origen", "Tareas");
        var tareas = new List<Tareas>();
        if(EsAdmin){ //Comprobamos si el usuario es admin para listar o no todas las tareas.
            tareas = repositorioTareas.ListarTareas();
        }else{
            var id = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            tareas = repositorioTareas.ListarTareasDeUsuario(id);
        }
        var tareasVM = tareas.Select(t =>{
            var nombreUsuarioAsignado = "Sin Asignar";
            if(t.idUsuarioAsignado!=-1 && t.idUsuarioAsignado!=null){
                nombreUsuarioAsignado = repositorioUsuarios.ObtenerDetallesDeUsuario((int)t.idUsuarioAsignado).nombreDeUsuario;
            }
            return new ListarTareasVM(t, nombreUsuarioAsignado);
        }).ToList(); //Detalle estético, uso esto para mostrar el nombre del usuario a quien se le asigno la tarea (y sino pongo sin asignar).
        return View(tareasVM);
    }
    [HttpGet]
    public IActionResult AltaTarea(int id){ //Vista para crear una nueva tarea recibiendo el id de un tablero. Solo se puede acceder si es dueño del tablero o admin.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
        var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
        var idDueño = tablero.idUsuarioPropietario;
        var EsDueño = idUsuario == idDueño;
        if(!EsDueño){ //Verificamos que el usuario sea dueño para poder acceder.
            TempData["ErrorMessage"] = "Acceso denegado";
            return RedirectToAction("VerTablero", "Tablero", new {id = id});
        }
        var tareaVM = new CrearTareaVM(id); 
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult CrearTarea(CrearTareaVM tareaVM){ //Método para crear una nueva tarea.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) RedirectToAction("AltaTarea");
        var tarea = new Tareas(tareaVM);
        repositorioTareas.CrearTarea(tarea.idTablero, tarea);
        return RedirectToAction("VerTablero", "Tablero", new {id = tarea.idTablero});
    }
    [HttpGet]
    public IActionResult ModificarTarea(int id){ //Vista para modificar una determinada tarea. Se puede acceder a ella solo si sos admin, dueño del tablero donde se encuentra dicha tarea o si fuiste asignado para ella tarea, sino no. 
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
        var tareaVM = new ModificarTareaVM(tarea);
        if(!EsAdmin){ //Verificamos si es admin para dar control total a la modificacion sin importar si es o no dueño, si no es admin se verifica que al menos sea dueño o esté asignado para acceder.
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            var idUsuarioAsignado = tarea.idUsuarioAsignado;
            var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.idTablero).idUsuarioPropietario;
            var EsDueño = idUsuario == idDueño;
            var EstaAsignado = idUsuario == idUsuarioAsignado;
            if(!EsDueño && !EstaAsignado){ //Verificamos si el usuario no es ni dueño ni asignado, si no es ninguna de las dos no accede.
                TempData["ErrorMessage"] = "Acceso denegado";
                return RedirectToAction("Index");
            }
            tareaVM = new ModificarTareaVM(tareaVM, EsDueño);
        }
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarTareaVM tareaVM){ //Método para modificar la tarea
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) RedirectToAction("ModificarTarea", new {id = tareaVM.id});
        var origen = HttpContext.Session.GetString("origen");
        var tarea = new Tareas(tareaVM);
        repositorioTareas.ModificarTarea(tarea.id,tarea);
        if(origen == "Tablero"){ //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede modificar desde el index de tareas o desde VerTablero() en el controller de tablero.
            return RedirectToAction("VerTablero", "Tablero", new {id = tareaVM.idTablero});
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult AsignarUsuario(int id){ //Vista para asignar o reasignar un usuario a una determinada tarea. Se puede acceder a esta vista siendo admin o dueño del tablero donde se encuentra la tarea, sino no.
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var origen = HttpContext.Session.GetString("origen");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        TempData["EsAdmin"] = EsAdmin;
        TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
        var listaDeUsuarios = repositorioUsuarios.ListarUsuarios();
        var listaDeUsuariosVM = listaDeUsuarios.Select(u => new ListarUsuariosVM(u)).ToList();
        if(!EsAdmin){ //Verificamos si el usuario no es admin, si no lo es procedemos a verificar si al menos es dueño.
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.idTablero).idUsuarioPropietario;
            var esDueño = idUsuario == idDueño;
            if(!esDueño){ //Si el usuario no es dueño se lo saca.
                TempData["ErrorMessage"] = "Acceso denegado";
                if(origen == "Tablero"){ //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede modificar desde el index de tareas o desde VerTablero() en el controller de tablero.
                    return RedirectToAction("VerTablero", "Tablero", new {id = tarea.idTablero});
                }else{
                    return RedirectToAction("Index");
                }
            }
        }
        var model = new AsignarUsuarioVM(tarea, listaDeUsuariosVM);
        return View(model);
    }
    [HttpPost]
    public IActionResult Asignar(AsignarUsuarioVM model){ //Método para asignar o reasignar un usuario a una determinada tarea.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction ("AsignarUsuario", new{id = model.idTarea});
        var origen = HttpContext.Session.GetString("origen");
        var tarea = new Tareas(model);
        repositorioTareas.AsignarUsuarioATarea(tarea.idUsuarioAsignado, tarea.id);
        if(origen == "Tablero"){ //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede asignar desde el index de tareas o desde VerTablero() en el controller de tablero.
            return RedirectToAction("VerTablero", "Tablero", new {id = model.idTablero});
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarTarea(int id){ //Vista para eliminar una determinada tarea. Se puede acceder a ella siendo admin o dueño del tablero, sino no.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        var origen = HttpContext.Session.GetString("origen");
        TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
        if(!EsAdmin){ //Verificamos si el usuario es admin, sino que al menos sea dueño.
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.idTablero).idUsuarioPropietario;
            var esDueño = idUsuario == idDueño;
            if(!esDueño){ //Si no es dueño se niega el acceso.
                TempData["ErrorMessage"] = "Acceso denegado";
                if(origen == "Tablero"){ //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede eliminar desde el index de tareas o desde VerTablero() en el controller de tablero.
                    return RedirectToAction("VerTablero", "Tablero", new {id=tarea.idTablero});
                }else{
                    return RedirectToAction("Index");
                }
            }
        }
        var tareaVM = new EliminarTareaVM(tarea);
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult Eliminar(EliminarTareaVM tareaVM){ //Método para eliminar una tarea determinada por id.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("EliminarTarea", new {id =tareaVM.id});
        var origen = HttpContext.Session.GetString("origen");
        var tarea = new Tareas(tareaVM);
        repositorioTareas.EliminarTarea(tarea.id);
        if(origen == "Tablero"){ //Verificamos de donde viene el usuario con una variable llamada "origen", ya que la tarea se puede eliminar desde el index de tareas o desde VerTablero() en el controller de tablero.
            return RedirectToAction("VerTablero", "Tablero", new {id = tarea.idTablero});
        }
        return RedirectToAction("Index");
    }
}