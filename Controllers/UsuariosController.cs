using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_Olme2.Models;
public class UsuariosController : Controller{
    private readonly ILogger<UsuariosController> _logger;
    private IUsuariosRepository repositorioUsuarios;
    public UsuariosController(ILogger<UsuariosController> logger, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        repositorioUsuarios = RepositorioUsuarios;
    }
    public IActionResult Index(){ //Vista principal de usuarios. Si el usuario es admin muestra todas las acciones para hacerse (eliminar, crear o modificar), si no lo es solo muestra los usuarios.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        ViewData["EsAdmin"] = HttpContext.Session.GetString("Rol") == "Administrador";
        var usuarios = repositorioUsuarios.ListarUsuarios();
        var usuariosVM = usuarios.Select(u => new ListarUsuariosVM(u)).ToList();
        return View(usuariosVM);
    }
    [HttpGet]
    public IActionResult AltaUsuario(){ //Vista para crear un nuevo usuario. Solo puede acceder a ella el admin.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var esAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        if(!esAdmin){ //Se verifica que el usuario sea admin, si no lo es se le niega el acceso.
            TempData["ErrorMessage"] = "Acceso Denegado";
            return RedirectToAction("Index");
        }
        return View();
    }
    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioVM usuarioVM){ //Método para crear un nuevo usuario.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("AltaUsuario");
        var usuario = new Usuarios(usuarioVM);
        repositorioUsuarios.CrearUsuario(usuario);
        return RedirectToAction("Index"); 
    }
    [HttpGet]
    public IActionResult ModificarUsuario(int id){ //Vista para modificar un determinado usuario. Solo puede acceder a ella el admin.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var esAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        if(!esAdmin){ //Se verifica que el usuario sea admin, si no lo es se le niega el acceso.
            TempData["ErrorMessage"] = "Acceso Denegado";
            return RedirectToAction("Index");
        }
        var usuario = repositorioUsuarios.ObtenerDetallesDeUsuario(id);
        var usuarioVM = new ModificarUsuarioVM(usuario);
        return View(usuarioVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarUsuarioVM usuarioVM){ //Método para modificar un determinado usuario.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("ModificarUsuario", new {id = usuarioVM.id});
        var usuario = new Usuarios(usuarioVM);
        repositorioUsuarios.ModificarUsuario(usuario.id, usuario);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarUsuario(int id){ //Vista para eliminar un determinado usuario. Solo puede acceder a ella el admin y no puede borrarse a si mismo.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var esAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        if(!esAdmin){ //Se verifica que el usuario sea admin, si no lo es se le niega el acceso.
            TempData["ErrorMessage"] = "Acceso Denegado";
            return RedirectToAction("Index");
        }
        var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
        if(idUsuario == id){ //Se verifica que el admin no se quiera borrar asi mismo ya que podría traer problemas. 
            TempData["ErrorMessage"] = "No se puede eliminar a si mismo";
            return RedirectToAction("Index");
        }
        var usuario = repositorioUsuarios.ObtenerDetallesDeUsuario(id);
        var usuarioVM = new EliminarUsuarioVM(usuario);
        return View(usuarioVM);
    }
    [HttpPost]
    public IActionResult Eliminar(EliminarUsuarioVM usuarioVM){ //Método para eliminar un determinado usuario.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("EliminarUsuario", new {id = usuarioVM.id});
        var usuario = new Usuarios(usuarioVM);
        repositorioUsuarios.EliminarUsuarioPorId(usuario.id);
        return RedirectToAction("Index");
    }
}