using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_proyecto_2024_Olme2.Models;
public class TareasController : Controller{
    private readonly ILogger<TareasController> _logger;
    private ITareasRepository repositorioTareas;
    private IUsuariosRepository repositorioUsuarios;
    private ITableroRepository repositorioTablero;
    public TareasController(ILogger<TareasController> logger, ITareasRepository RepositorioTareas, IUsuariosRepository RepositorioUsuarios, ITableroRepository RepositorioTablero){
        _logger = logger;
        repositorioTareas = RepositorioTareas;
        repositorioUsuarios = RepositorioUsuarios;
        repositorioTablero = RepositorioTablero;
    }
    public IActionResult Index(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 
        ViewData["EsAdmin"] = EsAdmin;
        HttpContext.Session.SetString("origen", "Tareas");
        var tareas = new List<Tareas>();
        if(EsAdmin){
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
        }).ToList();
        return View(tareasVM);
    }
    [HttpGet]
    public IActionResult AltaTarea(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var tareaVM = new CrearTareaVM(){idTablero = id};
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult CrearTarea(CrearTareaVM tareaVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) RedirectToAction("AltaTarea");
        var tarea = new Tareas(tareaVM);
        repositorioTareas.CrearTarea(tarea.idTablero, tarea);
        return RedirectToAction("VerTablero", "Tablero", tareaVM.idTablero);
    }
    [HttpGet]
    public IActionResult ModificarTarea(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
        var tareaVM = new ModificarTareaVM();
        if(EsAdmin){
            tareaVM = new ModificarTareaVM(tarea);
        }else{
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetString("Id"));
            var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.idTablero).idUsuarioPropietario;
            var EsDueño = idUsuario == idDueño;
            tareaVM = new ModificarTareaVM(tarea, EsDueño);
        }
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarTareaVM tareaVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) RedirectToAction("ModificarTarea", tareaVM.id);
        var origen = HttpContext.Session.GetString("origen");
        var tarea = new Tareas(tareaVM);
        repositorioTareas.ModificarTarea(tarea.id,tarea);
        if(origen == "Tablero"){
            return RedirectToAction("VerTablero", "Tablero", new {id = tareaVM.idTablero});
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult AsignarUsuario(int id){
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var origen = HttpContext.Session.GetString("origen");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        TempData["EsAdmin"] = EsAdmin;
        TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
        var model = new AsignarUsuarioVM();
        var listaDeUsuarios = repositorioUsuarios.ListarUsuarios();
        var listaDeUsuariosVM = listaDeUsuarios.Select(u => new ListarUsuariosVM(u)).ToList();
        if(!EsAdmin){
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetString("Id"));
            var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.id).idUsuarioPropietario;
            var esDueño = idUsuario == idDueño;
            if(!esDueño){
                TempData["ErrorMessage"] = "Acceso denegado";
                if(origen == "Tablero"){
                    return RedirectToAction("VerTablero", "Tablero", tarea.idTablero);
                }else{
                    return RedirectToAction("Index");
                }
            }
        }
        model = new AsignarUsuarioVM(tarea, listaDeUsuariosVM);
        return View(model);
    }
    [HttpPost]
    public IActionResult Asignar(AsignarUsuarioVM model){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction ("AsignarUsuario", model.idTarea);
        var origen = HttpContext.Session.GetString("origen");
        var tarea = new Tareas(model);
        repositorioTareas.AsignarUsuarioATarea(tarea.idUsuarioAsignado, tarea.id);
        if(origen == "Tablero"){
            return RedirectToAction("VerTablero", "Tablero", new {id = model.idTablero});
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarTarea(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        var origen = HttpContext.Session.GetString("origen");
        TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
        if(!EsAdmin){
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetString("Id"));
            var idDueño = repositorioTablero.ObtenerDetallesDeTablero(tarea.id).idUsuarioPropietario;
            var esDueño = idUsuario == idDueño;
            if(!esDueño){
                TempData["ErrorMessage"] = "Acceso denegado";
                if(origen == "Tablero"){
                    return RedirectToAction("VerTablero", "Tablero", tarea.idTablero);
                }else{
                    return RedirectToAction("Index");
                }
            }
        }
        var tareaVM = new EliminarTareaVM(tarea);
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult Eliminar(EliminarTareaVM tareaVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("EliminarTarea", tareaVM.id);
        var origen = HttpContext.Session.GetString("origen");
        var tarea = new Tareas(tareaVM);
        repositorioTareas.EliminarTarea(tarea.id);
        if(origen == "Tablero"){
            return RedirectToAction("VerTablero", "Tablero", tarea.idTablero);
        }
        return RedirectToAction("Index");
    }
}