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
            //Comprobamos la existencia del usuario.
            if(usuario != null){
                
                HttpContext.Session.SetString("Autenticado", "true");
                HttpContext.Session.SetInt32("Id", usuario.id);
                HttpContext.Session.SetString("Usuario", usuario.nombreDeUsuario);
                HttpContext.Session.SetString("Rol", usuario.rolUsuario.ToString());
                
                _logger.LogInformation($"El usuario {model.nombreDeUsuario} ingresó correctamente.");
                TempData["WelcomeMessage"] = "¡Bienvenido "+usuario.nombreDeUsuario+"!";
                return RedirectToAction("Index", "Tablero");
            
            }
            
            model.id = repositorioUsuarios.BuscarIdPorNombreDeUsuario(model.nombreDeUsuario);
            //Si el usuario es distinto de -1 quiere decir que existe, el método en el repositorio devuelve -1 si no existe.
            if(model.id!=-1){
            
                model.error = "Contraseña incorrecta.";
                //Se asigna esto momentáneamente para que no se pueda cambiar la contraseña de cualquier usuario, solo si puso correctamente el nombre.
                HttpContext.Session.SetInt32("Id", model.id);

            }else{
                
                model.error = "Usuario inexistente.";
            
            }
            
            _logger.LogWarning($"Intento de acceso inválido - Usuario: {model.nombreDeUsuario} Clave ingresada: {model.password}");
            model.autenticado = false;
            return View("Index", model);

        }catch(Exception e){
            
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

    [HttpGet]
    //Vista para cambiar contraseña. Solo deja acceder si se puso correctamente el nombre de usuario, sino salta error.
    public IActionResult CambiarPassword(int id){ 
        try{
        
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            var usuario = repositorioUsuarios.ObtenerDetallesDeUsuario(id);

            //Comprobamos que el usuario haya intentado ingresar el nombre de usuario al menos.
            if(idUsuario != usuario.id){
            
                TempData["ErrorMessage"] = "Acceso Denegado.";
                return RedirectToAction("Index");

            }

            var usuarioVM = new CambiarPasswordVM(usuario);
            return View(usuarioVM);
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home");
        
        }
    }

    [HttpPost]
    //Metodo para cambiar la contraseña, la cambia solamente si coinciden la contraseña y "Repetir contraseña".
    public IActionResult Cambiar(CambiarPasswordVM usuarioVM){ 
        try{
            
            if (!ModelState.IsValid) return RedirectToAction("CambiarPassword", usuarioVM.id);

            var usuario = new Usuarios(usuarioVM);
            repositorioUsuarios.CambiarPassword(usuario.id, usuario.password);
            TempData["Mensaje"] = "¡Contraseña cambiada exitosamente!";

            return RedirectToAction("Index");
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return BadRequest("No se cambió correctamente la contraseña.");

        }
    }
}