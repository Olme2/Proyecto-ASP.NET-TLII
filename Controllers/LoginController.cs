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
            autenticado = HttpContext.Session.GetString("autenticado") == "true"
        };
        return View(model);
    }
    public IActionResult Login(LoginVM model){
        Usuarios usuario = _repositorioUsuarios.ObtenerUsuarioPorNombreYPassword(model.nombreDeUsuario, model.password);
        if(usuario != null){
            HttpContext.Session.SetString("Autenticado", "true");
            HttpContext.Session.SetInt32("Id", usuario.id);
            HttpContext.Session.SetString("Usuario", usuario.nombreDeUsuario);
            HttpContext.Session.SetString("Rol", usuario.rolUsuario.ToString());
            return RedirectToAction("Index", "Tablero");
        }
        model.error = "Usuario o contraseña incorrectos";
        model.autenticado = false;
        return View("Index", model);
    }
    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}