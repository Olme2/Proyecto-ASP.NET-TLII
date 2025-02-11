using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_Olme2.Models;
public class UsuariosController : Controller{
    private readonly ILogger<UsuariosController> _logger;
    private IUsuariosRepository repositorioUsuarios;
    public UsuariosController(ILogger<UsuariosController> logger, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        repositorioUsuarios = RepositorioUsuarios;
    }
    public IActionResult Index(){
        var usuarios = repositorioUsuarios.ListarUsuarios();
        var usuariosVM = usuarios.Select(u => new ListarUsuariosVM(u)).ToList;
        return View(usuariosVM);
    }
    [HttpGet]
    public IActionResult AltaUsuario(){
        return View();
    }
    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioVM usuarioVM){
        var usuario = new Usuarios(usuarioVM);
        repositorioUsuarios.CrearUsuario(usuario);
        return RedirectToAction("Index"); 
    }
    [HttpGet]
    public IActionResult ModificarUsuario(int id){
        var usuario = repositorioUsuarios.ObtenerDetallesDeUsuario(id);
        var usuarioVM = new ModificarUsuarioVM(usuario);
        return View(usuarioVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarUsuarioVM usuarioVM){
        var usuario = new Usuarios(usuarioVM);
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