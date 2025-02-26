using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_proyecto_2024_Olme2.Models;
public class TareasController : Controller{
    private readonly ILogger<TareasController> _logger;
    private ITareasRepository repositorioTareas;
    private IUsuariosRepository repositorioUsuarios;
    public TareasController(ILogger<TareasController> logger, ITareasRepository RepositorioTareas, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        repositorioTareas = RepositorioTareas;
        repositorioUsuarios = RepositorioUsuarios;
    }
    public IActionResult Index(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        HttpContext.Session.SetString("origen", "Tareas");
        var id = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
        var tareas = repositorioTareas.ListarTareasDeUsuario(id);
        var tareasVM = tareas.Select(t => new ListarTareasVM(t)).ToList();
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
        var tarea = new Tareas(tareaVM);
        repositorioTareas.CrearTarea(tarea.idTablero, tarea);
        return RedirectToAction("Index", "Tablero");
    }
    [HttpGet]
    public IActionResult ModificarTarea(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
        var tareaVM = new ModificarTareaVM(tarea);
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarTareaVM tareaVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        string? origen = HttpContext.Session.GetString("origen");
        var tarea = new Tareas(tareaVM);
        repositorioTareas.ModificarTarea(tarea.id,tarea);
        if(origen == "Tablero"){
            return RedirectToAction("VerTablero", "Tablero", new {id = tareaVM.idTablero});
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult AsignarUsuario(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(id);
        var model = new AsignarUsuarioVM(tarea);
        model.listaDeUsuarios = repositorioUsuarios.ListarUsuarios();
        return View(model);
    }
    [HttpPost]
    public IActionResult Asignar(AsignarUsuarioVM model){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var tarea = new Tareas(model);
        repositorioTareas.AsignarUsuarioATarea(tarea.idUsuarioAsignado, tarea.id);
        return RedirectToAction("VerTablero", "Tablero", new {id= tarea.idTablero});
    }
    [HttpGet]
    public IActionResult EliminarTarea(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        TempData["PreviousUrl"] = Request.Headers["Referer"].ToString();
        ViewData["idTablero"] = repositorioTareas.ObtenerDetallesDeTarea(id).idTablero;
        return View(id);
    }
    [HttpGet]
    public IActionResult Eliminar(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        string? origen = HttpContext.Session.GetString("origen");
        repositorioTareas.EliminarTarea(id);
        if(origen == "Tablero"){
            return RedirectToAction("VerTablero", "Tablero", new {id = Convert.ToInt32(TempData["idTablero"])});
        }
        return RedirectToAction("Index");
    }
}