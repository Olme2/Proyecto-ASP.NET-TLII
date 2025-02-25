using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_proyecto_2024_Olme2.Models;
public class TableroController : Controller{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository repositorioTablero;
    private IUsuariosRepository repositorioUsuarios;
    private ITareasRepository repositorioTareas;
    public TableroController(ILogger<TableroController> logger, ITableroRepository ReposiotrioTablero, IUsuariosRepository RepositorioUsuarios, ITareasRepository RepositorioTareas){
        _logger = logger;
        repositorioTablero = ReposiotrioTablero;
        repositorioUsuarios = RepositorioUsuarios;
        repositorioTareas = RepositorioTareas;
    }
    public IActionResult Index(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
        var tableros = repositorioTablero.ListarTablerosConTareasAsignadas(idUsuario);
        var tablerosVM = tableros.Select(t => new ListarTablerosVM(t)).ToList();
        var tablerosUsuario = repositorioTablero.ListarTablerosDeUsuario(idUsuario);
        var tablerosUsuarioVM = tablerosUsuario.Select(t => new ListarTablerosVM(t)).ToList();
        ViewData["tablerosUsuario"] = tablerosUsuarioVM;
        return View(tablerosVM);
    }
    public IActionResult VerTablero(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        HttpContext.Session.SetString("origen", "Tablero");
        int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
        var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
        var UsuarioPropietario = repositorioUsuarios.ObtenerDetallesDeUsuario(tablero.idUsuarioPropietario);
        var tableroVM = new ListarTablerosVM(tablero);
        var tareas = repositorioTareas.ListarTareasDeTablero(id);
        var tareasVM = tareas.Select(t => new ListarTareasVM(t)).ToList();
        ViewData["idUsuario"] = idUsuario;
        ViewData["Tareas"] = tareasVM;
        ViewData["UsuarioPropietario"] = UsuarioPropietario;
        return View(tableroVM);
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
        var tareas = repositorioTareas.ListarTareasDeTablero(id);
        if(tareas.Count>0){
            ViewData["ErrorMessage"] = "No se puede borrar el tablero porque tiene tareas cargadas.";
        }
        return View(id);
    }
    [HttpGet]
    public IActionResult Eliminar(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        repositorioTablero.EliminarTableroPorId(id);
        return RedirectToAction("Index");
    }
}