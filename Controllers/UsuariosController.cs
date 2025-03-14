using Microsoft.AspNetCore.Mvc;
using tl2_proyecto_2024_Olme2.Models;
public class UsuariosController : Controller{

    private readonly ILogger<UsuariosController> _logger;
    private IUsuariosRepository repositorioUsuarios;
    //Agregado para verificar si un usuario tiene tableros.
    private ITableroRepository repositorioTablero;
    //Agregado para verificar si un usuario tiene tareas asignadas.
    private ITareasRepository repositorioTareas;
    
    public UsuariosController(ILogger<UsuariosController> logger, IUsuariosRepository RepositorioUsuarios, ITableroRepository RepositorioTablero, ITareasRepository RepositorioTareas){
        _logger = logger;
        repositorioUsuarios = RepositorioUsuarios;
        repositorioTablero = RepositorioTablero;
        repositorioTareas = RepositorioTareas;
    }
    
    //Vista principal de usuarios. Si el usuario es admin muestra todas las acciones para hacerse (eliminar, crear o modificar), si no lo es solo muestra los usuarios.
    public IActionResult Index(){ 
        try{ 
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            
            ViewData["EsAdmin"] = HttpContext.Session.GetString("Rol") == "Administrador";

            var usuarios = repositorioUsuarios.ListarUsuarios();
            var usuariosVM = usuarios.Select(u => new ListarUsuariosVM(u)).ToList();

            return View(usuariosVM);

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "no cargaron correctamente los usuarios."});

        }
    }

    [HttpGet]
    //Vista para crear un nuevo usuario. Solo puede acceder a ella el admin.
    public IActionResult AltaUsuario(){ 
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var esAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
            //Se verifica que el usuario sea admin, si no lo es se le niega el acceso.
            if(!esAdmin){ 
                TempData["ErrorMessage"] = "Acceso Denegado.";
                return RedirectToAction("Index");
            }

            return View();

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "no cargó correctamente la página."});

        }
    }

    [HttpPost]
    //Método para crear un nuevo usuario.
    public IActionResult CrearUsuario(CrearUsuarioVM usuarioVM){ 
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) return RedirectToAction("AltaUsuario");

            var nombresDeUsuario = repositorioUsuarios.ListarUsuarios().Select(u => u.nombreDeUsuario).ToList();

            //Se verifica que no se haya usado un nombre de usuario ya existente, ya que como cada nombre de usuario es único, no se podria hacer eso.
            if(nombresDeUsuario.Contains(usuarioVM.nombreDeUsuario)){
                TempData["ErrorMessage"] = "El nombre de usuario ya existe, use otro.";
                return RedirectToAction("AltaUsuario");
            }

            var usuario = new Usuarios(usuarioVM);
            repositorioUsuarios.CrearUsuario(usuario);

            _logger.LogInformation($"Usuario "+usuario.nombreDeUsuario+" creado correctamente.");
            TempData["SuccessMessage"] = "¡Usuario \""+usuarioVM.nombreDeUsuario+"\" creado con éxito!";    
            return RedirectToAction("Index"); 
    
        }catch(Exception e){

            _logger.LogWarning("Creación de usuario sin éxito.");
            _logger.LogError(e.ToString());
            return BadRequest("Creación de usuario sin éxito.");

        }
    }

    [HttpGet]
    //Vista para modificar un determinado usuario. Solo puede acceder a ella el admin.
    public IActionResult ModificarUsuario(int id){ 
        try{

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var esAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
            //Se verifica que el usuario sea admin, si no lo es se le niega el acceso.
            if(!esAdmin){ 
                TempData["ErrorMessage"] = "Acceso Denegado.";
                return RedirectToAction("Index");
            }

            var usuario = repositorioUsuarios.ObtenerDetallesDeUsuario(id);
            var usuarioVM = new ModificarUsuarioVM(usuario);

            return View(usuarioVM);

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "el usuario solicitado no existe."});

        }
    }

    [HttpPost]
    //Método para modificar un determinado usuario.
    public IActionResult Modificar(ModificarUsuarioVM usuarioVM){
        try{ 

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) return RedirectToAction("ModificarUsuario", new {id = usuarioVM.id});

            var usuario = new Usuarios(usuarioVM);
            repositorioUsuarios.ModificarUsuario(usuario.id, usuario);

            _logger.LogInformation($"Usuario "+usuario.nombreDeUsuario+" modificado correctamente.");
            TempData["SuccessMessage"] = "¡Usuario \""+usuarioVM.nombreDeUsuario+"\" modificado con éxito!";
            return RedirectToAction("Index");

        }catch(Exception e){

            _logger.LogError("Modificación de usuario sin éxito");
            _logger.LogError(e.ToString());
            return BadRequest("Modificación de usuario sin éxito");
        }
    }

    [HttpGet]
    //Vista para eliminar un determinado usuario. Solo puede acceder a ella el admin y no puede borrarse a si mismo.
    public IActionResult EliminarUsuario(int id){ 
        try{

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var esAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
            //Se verifica que el usuario sea admin, si no lo es se le niega el acceso.
            if(!esAdmin){ 
                TempData["ErrorMessage"] = "Acceso Denegado";
                return RedirectToAction("Index");
            }
            
            var tablerosDeUsuario = repositorioTablero.ListarTablerosDeUsuario(id);
            var tareasDeUsuario = repositorioTareas.ListarTareasDeUsuario(id);
            //Se verifica que el usuario no tenga ningun tablero ni tarea asignada
            if(tablerosDeUsuario.Count>0 || tareasDeUsuario.Count>0){
                TempData["ErrorMessage"] = "No se puede eliminar un usuario que tenga tableros a su nombre o tareas designadas.";
                return RedirectToAction("Index");
            }

            var idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            //Se verifica que el admin no se quiera borrar asi mismo ya que podría traer problemas. 
            if(idUsuario == id){ 
                TempData["ErrorMessage"] = "No se puede eliminar a si mismo";
                return RedirectToAction("Index");
            }

            var usuario = repositorioUsuarios.ObtenerDetallesDeUsuario(id);
            var usuarioVM = new EliminarUsuarioVM(usuario);

            return View(usuarioVM);

        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "el usuario solicitado no existe."});

        }
    }

    [HttpPost]
    //Método para eliminar un determinado usuario.
    public IActionResult Eliminar(EliminarUsuarioVM usuarioVM){ 
        try{

            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if(!ModelState.IsValid) return RedirectToAction("EliminarUsuario", new {id = usuarioVM.id});

            var usuario = new Usuarios(usuarioVM);
            repositorioUsuarios.EliminarUsuarioPorId(usuario.id);

            _logger.LogInformation($"Usuario "+usuario.nombreDeUsuario+" eliminado correctamente.");
            TempData["SuccessMessage"] = "¡Usuario \""+usuarioVM.nombreDeUsuario+"\" eliminado con éxito!";
            return RedirectToAction("Index");

        }catch(Exception e){

            _logger.LogWarning("Eliminación de usuario sin éxito.");
            _logger.LogError(e.ToString());
            return BadRequest("Eliminación de usuario sin éxito.");

        }
    }

    [HttpGet]
    //Vista para cambiar contraseña. Solo deja acceder si es admin.
    public IActionResult CambiarPassword(int id){ 
        try{
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");

            var esAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
            //Se verifica que el usuario sea admin, si no lo es se le niega el acceso.
            if(!esAdmin){ 
                TempData["ErrorMessage"] = "Acceso Denegado";
                return RedirectToAction("Index");
            }

            var usuario = repositorioUsuarios.ObtenerDetallesDeUsuario(id);
            var usuarioVM = new CambiarPasswordVM(usuario);
            return View(usuarioVM);
        
        }catch(Exception e){

            _logger.LogError(e.ToString());
            return RedirectToAction("Error", "Home", new {error = "el usuario solicitado no existe"});
        
        }
    }

    [HttpPost]
    //Metodo para cambiar la contraseña, la cambia solamente si coinciden la contraseña y "Repetir contraseña".
    public IActionResult Cambiar(CambiarPasswordVM usuarioVM){ 
        try{
            
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
            if (!ModelState.IsValid) return RedirectToAction("CambiarPassword", usuarioVM.id);

            var usuario = new Usuarios(usuarioVM);
            repositorioUsuarios.CambiarPassword(usuario.id, usuario.password);

            _logger.LogInformation($"Contraseña de usuario "+usuario.nombreDeUsuario+" cambiada correctamente.");
            TempData["SuccessMessage"] = "¡Contraseña cambiada exitosamente!";
            return RedirectToAction("Index");
        
        }catch(Exception e){

            _logger.LogWarning("No se cambió correctamente la contraseña.");
            _logger.LogError(e.ToString());
            return BadRequest("No se cambió correctamente la contraseña."); //

        }
    }
}
