using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_Olme2.Models;
public class TableroController : Controller{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository repositorioTablero;
    public TableroController(ILogger<TableroController> logger, ITableroRepository ReposiotrioTablero){
        _logger = logger;
        repositorioTablero = ReposiotrioTablero;
    }
    public IActionResult Index(){
        return View(repositorioTablero.ListarTableros());
    }
    [HttpGet]
    public IActionResult AltaTablero(){
        return View();
    }
    [HttpPost]
    public IActionResult CrearTablero(CrearTableroVM tableroVM){
        var tablero = new Tablero(tableroVM);
        repositorioTablero.CrearTablero(tablero);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarTablero(int id){
        return View(repositorioTablero.ObtenerDetallesDeTablero(id));
    }
    [HttpPost]
    public IActionResult Modificar(ModificarTableroVM tableroVM){
        var tablero = new Tablero(tableroVM);
        repositorioTablero.ModificarTablero(tablero.id, tablero);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarTablero(int id){
        return View(id);
    }
    [HttpPost]
    public IActionResult Eliminar(int id){
        repositorioTablero.EliminarTableroPorId(id);
        return RedirectToAction("Index");
    }
}