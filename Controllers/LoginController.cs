using Microsoft.AspNetCore.Mvc;

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
        Usuarios usuario = _repositorioUsuarios.ObtenerDetallesDeUsuario(model.id);
        if(usuario != null){
            HttpContext.Session.SetString("Autenticado", "true");
            HttpContext.Session.SetString("Usuario", usuario.nombreDeUsuario);
            HttpContext.Session.SetString("Rol", usuario.rolUsuario.ToString());
            return RedirectToAction("Index", "Tableros");
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