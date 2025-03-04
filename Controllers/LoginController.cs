using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_Olme2.Models;
public class LoginController : Controller{

    private readonly ILogger<LoginController> _logger;
    private readonly IUsuariosRepository repositorioUsuarios;

    public LoginController(ILogger<LoginController> logger, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        repositorioUsuarios = RepositorioUsuarios;
    }

    //Vista principal, es la primera vista que aparece cuando se ingresa a la app, el formulario de inicio de sesión.
    public IActionResult Index(){
        try{
            
            var model = new LoginVM{
                autenticado = HttpContext.Session.GetString("autenticado") == "true",
                error = string.Empty
            };
            
            return View(model);

        }catch(Exception e){
            
            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");
        
        }
    }

    //Metodo para verificar inicio de sesión. Si el usuario y contraseña son correctos, ingresa al sistema. Si el usuario no existe se le avisa al usuario. Si la contraseña esta mal, se abre un link para cambiarla.
    public IActionResult Login(LoginVM model){
        try{

            if(!ModelState.IsValid) return RedirectToAction("Index");
            
            var usuario = repositorioUsuarios.ObtenerUsuarioPorNombreYPassword(model.nombreDeUsuario, model.password);
            //Verificamos que el usuario no sea nulo.
            if(usuario==null){
                TempData["ErrorMessage"] = "Datos incorrectos";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("Autenticado", "true");
            HttpContext.Session.SetInt32("Id", usuario.id);
            HttpContext.Session.SetString("Usuario", usuario.nombreDeUsuario);
            HttpContext.Session.SetString("Rol", usuario.rolUsuario.ToString());

            _logger.LogInformation($"El usuario {model.nombreDeUsuario} ingresó correctamente.");
            
            TempData["WelcomeMessage"] = "¡Bienvenido "+usuario.nombreDeUsuario+"!";
            return RedirectToAction("Index", "Tablero");
                        
        }catch(Exception e){
            
            _logger.LogWarning($"Intento de acceso inválido - Usuario: {model.nombreDeUsuario} Clave ingresada: {model.password}");
            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");
        
        }
    }

    //Metodo para salir del inicio de sesión
    public IActionResult Logout(){ 
        try{

            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Sesión cerrada correctamente.";
            return RedirectToAction("Index");
        
        }catch(Exception e){

            _logger.LogError(e.Message);
            return RedirectToAction("Error", "Home");

        }
    }

}