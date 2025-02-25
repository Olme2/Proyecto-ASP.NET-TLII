using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_Olme2.Models;
public class LoginController : Controller{
    private readonly ILogger<LoginController> _logger;
    private readonly IUsuariosRepository _repositorioUsuarios;
    public LoginController(ILogger<LoginController> logger, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        _repositorioUsuarios = RepositorioUsuarios;
    }
    public IActionResult Index(){
        var model = new LoginVM{
            autenticado = HttpContext.Session.GetString("autenticado") == "true",
            error = string.Empty
        };
        return View(model);
    }
    public IActionResult Login(LoginVM model){
        Usuarios? usuario = _repositorioUsuarios.ObtenerUsuarioPorNombreYPassword(model.nombreDeUsuario, model.password);
        if(usuario != null){
            HttpContext.Session.SetString("Autenticado", "true");
            HttpContext.Session.SetInt32("Id", usuario.id);
            HttpContext.Session.SetString("Usuario", usuario.nombreDeUsuario);
            HttpContext.Session.SetString("Rol", usuario.rolUsuario.ToString());
            return RedirectToAction("Index", "Tablero");
        }
        model.id = _repositorioUsuarios.BuscarIdPorNombreDeUsuario(model.nombreDeUsuario);
        if(model.id!=-1){ 
            model.error = "Contraseña incorrecta";
        }else{
            model.error = "Usuario inexistente";
        }
        model.autenticado = false;
        return View("Index", model);
    }
    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult CambiarPassword(int id){
        var usuario = _repositorioUsuarios.ObtenerDetallesDeUsuario(id);
        var usuarioVM = new CambiarPasswordVM(usuario);
        return View(usuarioVM);
    }
    [HttpPost]
    public IActionResult Cambiar(CambiarPasswordVM usuarioVM){
        if (!ModelState.IsValid){
            return View("CambiarPassword", usuarioVM);
        }
        var usuario = new Usuarios(usuarioVM);
        _repositorioUsuarios.CambiarPassword(usuario.id, usuario.password);
        TempData["Mensaje"] = "¡Contraseña cambiada exitosamente!";
        return RedirectToAction("Index");
    }
}