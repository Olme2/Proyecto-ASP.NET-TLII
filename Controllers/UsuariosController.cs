using Microsoft.AspNetCore.Mvc;
public class UsuariosController : Controller{
    private readonly ILogger<UsuariosController> _logger;
    private IUsuariosRepository repositorioUsuarios;
    public UsuariosController(ILogger<UsuariosController> logger, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        repositorioUsuarios = RepositorioUsuarios;
    }
    public IActionResult Index(){
        return View(repositorioUsuarios.ListarUsuarios());
    }
    [HttpGet]
    public IActionResult AltaUsuario(){
        return View();
    }
    [HttpPost]
    public IActionResult CrearUsuario(Usuarios usuario){
       repositorioUsuarios.CrearUsuario(usuario);
       return RedirectToAction("Index"); 
    }
    [HttpGet]
    public IActionResult ModificarUsuario(int id){
        return View(repositorioUsuarios.ObtenerDetallesDeUsuario(id));
    }
    [HttpPost]
    public IActionResult Modificar(Usuarios usuario){
        repositorioUsuarios.ModificarUsuario(usuario.id, usuario);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarUsuario(int id){
        return View(id);
    }
    [HttpPost]
    public IActionResult Eliminar(int id){
        repositorioUsuarios.EliminarUsuarioPorId(id);
        return RedirectToAction("Index");
    }
}