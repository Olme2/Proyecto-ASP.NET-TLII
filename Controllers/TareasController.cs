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
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarTarea(int idTarea){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        List<Usuarios> usuarios = repositorioUsuarios.ListarUsuarios();
        ViewData["Usuarios"] = usuarios.Select(u=> new SelectListItem
        {
            Value = u.id.ToString(), 
            Text = u.nombreDeUsuario
        }).ToList();
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(idTarea);
        var tareaVM = new ModificarTareaVM(tarea);
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarTareaVM tareaVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var tarea = new Tareas(tareaVM);
        repositorioTareas.ModificarTarea(tarea.id,tarea);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarTarea(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        return View(id);
    }
    [HttpPost]
    public IActionResult Eliminar(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        repositorioTareas.EliminarTarea(id);
        return RedirectToAction("Index");
    }
}