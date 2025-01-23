using Microsoft.AspNetCore.Mvc;
public class TareasController : Controller{
    private readonly ILogger<TareasController> _logger;
    private ITareasRepository repositorioTareas;
    public TareasController(ILogger<TareasController> logger, ITareasRepository RepositorioTareas){
        _logger = logger;
        repositorioTareas = RepositorioTareas;
    }
    public IActionResult Index(int idUsuario){
        return View(repositorioTareas.ListarTareasDeUsuario(idUsuario));
    }
    [HttpGet]
    public IActionResult AltaTarea(){
        return View();
    }
    [HttpPost]
    public IActionResult CrearTarea(int idTablero, Tareas tarea){
        repositorioTareas.CrearTarea(idTablero, tarea);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarTarea(int idTarea){
        return View(repositorioTareas.ObtenerDetallesDeTarea(idTarea));
    }
    [HttpPost]
    public IActionResult Modificar(Tareas tarea){
        repositorioTareas.ModificarTarea(tarea.id,tarea);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarTarea(int id){
        return View(id);
    }
    [HttpPost]
    public IActionResult Eliminar(int id){
        repositorioTareas.EliminarTarea(id);
        return RedirectToAction("Index");
    }
}