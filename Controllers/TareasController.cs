using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_Olme2.Models;
public class TareasController : Controller{
    private readonly ILogger<TareasController> _logger;
    private ITareasRepository repositorioTareas;
    public TareasController(ILogger<TareasController> logger, ITareasRepository RepositorioTareas){
        _logger = logger;
        repositorioTareas = RepositorioTareas;
    }
    public IActionResult Index(){
        var id = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
        var tareas = repositorioTareas.ListarTareasDeUsuario(id);
        var tareasVM = tareas.Select(t => new ListarTareasVM(t)).ToList;
        return View(tareasVM);
    }
    [HttpGet]
    public IActionResult AltaTarea(int IdTablero){
        var tarea = new CrearTareaVM(){idTablero = IdTablero};
        return View(tarea);
    }
    [HttpPost]
    public IActionResult CrearTarea(CrearTareaVM tareaVM){
        var tarea = new Tareas(tareaVM);
        repositorioTareas.CrearTarea(tareaVM.idTablero, tarea);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarTarea(int idTarea){
        var tarea = repositorioTareas.ObtenerDetallesDeTarea(idTarea);
        var tareaVM = new ModificarTareaVM(tarea);
        return View(tareaVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarTareaVM tareaVM){
        var tarea = new Tareas(tareaVM);
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