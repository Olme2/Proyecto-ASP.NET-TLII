using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_Olme2.Models;
public class LoginController : Controller{
    private readonly ILogger<LoginController> _logger;
    private readonly IUsuariosRepository repositorioUsuarios;
    public LoginController(ILogger<LoginController> logger, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        repositorioUsuarios = RepositorioUsuarios;
    }
    public IActionResult Index(){ //Vista principal, es la primera vista que aparece cuando se ingresa a la app, el formulario de inicio de sesión.
        var model = new LoginVM{
            autenticado = HttpContext.Session.GetString("autenticado") == "true",
            error = string.Empty
        };
        return View(model);
    }
    public IActionResult Login(LoginVM model){ //Metodo para verificar inicio de sesión. Si el usuario y contraseña son correctos, ingresa al sistema. Si el usuario no existe se le avisa al usuario. Si la contraseña esta mal, se abre un link para cambiarla.
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var usuario = repositorioUsuarios.ObtenerUsuarioPorNombreYPassword(model.nombreDeUsuario, model.password);
        if(usuario != null){ //Comprobamos la existencia del usuario.
            HttpContext.Session.SetString("Autenticado", "true");
            HttpContext.Session.SetInt32("Id", usuario.id);
            HttpContext.Session.SetString("Usuario", usuario.nombreDeUsuario);
            HttpContext.Session.SetString("Rol", usuario.rolUsuario.ToString());
            return RedirectToAction("Index", "Tablero");
        }
        model.id = repositorioUsuarios.BuscarIdPorNombreDeUsuario(model.nombreDeUsuario);
        if(model.id!=-1){  //Si el usuario es distinto de -1 quiere decir que existe, el método en el repositorio devuelve -1 si no existe.
            model.error = "Contraseña incorrecta";
            HttpContext.Session.SetInt32("Id", model.id); //Se asigna esto momentáneamente para que no se pueda cambiar la contraseña de cualquier usuario, solo si puso correctamente el nombre.
        }else{
            model.error = "Usuario inexistente";
        }
        model.autenticado = false;
        return View("Index", model);
    }
    public IActionResult Logout(){ //Metodo para salir del inicio de sesión
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult CambiarPassword(int id){ //Vista para cambiar contraseña. Solo deja acceder si se puso correctamente el nombre de usuario, sino salta error.
        var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
        var usuario = repositorioUsuarios.ObtenerDetallesDeUsuario(id);
        if(idUsuario != usuario.id){
            TempData["ErrorMessage"] = "Acceso Denegado";
            return RedirectToAction("Index");
        }
        var usuarioVM = new CambiarPasswordVM(usuario);
        return View(usuarioVM);
    }
    [HttpPost]
    public IActionResult Cambiar(CambiarPasswordVM usuarioVM){ //Metodo para cambiar la contraseña, la cambia solamente si coinciden la contraseña y "Repetir contraseña".
        if (!ModelState.IsValid) return RedirectToAction("CambiarPassword", usuarioVM.id);
        var usuario = new Usuarios(usuarioVM);
        repositorioUsuarios.CambiarPassword(usuario.id, usuario.password);
        TempData["Mensaje"] = "¡Contraseña cambiada exitosamente!";
        return RedirectToAction("Index");
    }
}