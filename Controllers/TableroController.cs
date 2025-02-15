using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_proyecto_2024_Olme2.Models;
public class TableroController : Controller{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository repositorioTablero;
    private IUsuariosRepository repositorioUsuarios;
    public TableroController(ILogger<TableroController> logger, ITableroRepository ReposiotrioTablero, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        repositorioTablero = ReposiotrioTablero;
        repositorioUsuarios = RepositorioUsuarios;
    }
    public IActionResult Index(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var tableros = repositorioTablero.ListarTableros();
        var tablerosVM = tableros.Select(t => new ListarTablerosVM(t)).ToList();
        return View(tablerosVM);
    }
    [HttpGet]
    public IActionResult AltaTablero(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        ViewData["IdUsuario"] = HttpContext.Session.GetInt32("Id");
        return View();
    }
    [HttpPost]
    public IActionResult CrearTablero(CrearTableroVM tableroVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var tablero = new Tablero(tableroVM);
        repositorioTablero.CrearTablero(tablero);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarTablero(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var usuarios = repositorioUsuarios.ListarUsuarios();
        ViewData["Usuarios"] = usuarios.Select(u=> new SelectListItem
        {
            Value = u.id.ToString(), 
            Text = u.nombreDeUsuario
        }).ToList();
        var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
        var tableroVM = new ModificarTableroVM(tablero);
        return View(tableroVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarTableroVM tableroVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var tablero = new Tablero(tableroVM);
        repositorioTablero.ModificarTablero(tablero.id, tablero);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarTablero(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        return View(id);
    }
    [HttpPost]
    public IActionResult Eliminar(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        repositorioTablero.EliminarTableroPorId(id);
        return RedirectToAction("Index");
    }
}