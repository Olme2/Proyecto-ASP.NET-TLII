using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_proyecto_2024_Olme2.Models;
public class TableroController : Controller{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository repositorioTablero;
    private IUsuariosRepository repositorioUsuarios; //Agregado para operaciones como designar usuarios como propietarios de tableros.
    private ITareasRepository repositorioTareas; //Agregado para operaciones como mostrar las tareas en VerTablero().
    public TableroController(ILogger<TableroController> logger, ITableroRepository ReposiotrioTablero, IUsuariosRepository RepositorioUsuarios, ITareasRepository RepositorioTareas){
        _logger = logger;
        repositorioTablero = ReposiotrioTablero;
        repositorioUsuarios = RepositorioUsuarios;
        repositorioTareas = RepositorioTareas;
    }
    public IActionResult Index(){ //Vista principal, si el usuario es admin se muestran todos los tableros, sino se muestran los tableros creados por ellos primero y luego donde hay tareas asignadas.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login"); 
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 
        ViewData["EsAdmin"] = EsAdmin; 
        var tablerosVM = new List<ListarTablerosVM>();
        if(EsAdmin){ //Verificacion de admin, para ver si se manda todos los tableros a la lista, o solo los creados y con tareas asignadas.
            var tableros = repositorioTablero.ListarTableros(); 
            tablerosVM = tableros.Select(t => new ListarTablerosVM(t)).ToList();
        }else{
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id")); 
            var tableros = repositorioTablero.ListarTablerosConTareasAsignadas(idUsuario); //Obtenemos solo los tableros con tareas asignadas para cierto usuario.
            tablerosVM = tableros.Select(t => new ListarTablerosVM(t)).ToList();
            var tablerosUsuario = repositorioTablero.ListarTablerosDeUsuario(idUsuario); //Obtenemos solo los tableros creados por cierto usuario.
            var tablerosUsuarioVM = tablerosUsuario.Select(t => new ListarTablerosVM(t)).ToList();
            ViewData["tablerosUsuario"] = tablerosUsuarioVM; //Se envia por ViewData ya que los otros tableros los mandamos por vista.
        }
        return View(tablerosVM);
    }    
    public IActionResult VerTablero(int id){ //Vista detallada de un tablero en especifico. Si es admin puede acceder, sino solo se accede si sos dueño o tenes tareas asignadas.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login"); 
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 
        HttpContext.Session.SetString("origen", "Tablero"); //Se crea una variable llamada "origen" para guardar la url anterior, ya que luego si accedemos a una tarea, se puede acceder desde dos lugares distintos.
        var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
        if(tablero.nombre == string.Empty){ //Verificamos si el tablero existe, si no existe se redirecciona a Index.
            TempData["ErrorMessage"] = "El tablero no existe";
            return RedirectToAction("Index", "Tablero");
        }
        var tareas = repositorioTareas.ListarTareasDeTablero(id);
        var tareasVM = tareas.Select(t => new ListarTareasVM(t)).ToList();
        var usuarioPropietario = repositorioUsuarios.ObtenerDetallesDeUsuario(tablero.idUsuarioPropietario);
        var tableroVM = new VerTableroVM(tablero, tareasVM, usuarioPropietario.nombreDeUsuario); //Si es admin se obtienen todos los datos del tablero, de todas las tareas y el nombre del dueño.
        if(!EsAdmin){ //Verificamos si es admin para agregar acciones a todas las tareas o a solo las asignadas.
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            var tablerosConTareasAsignadas = repositorioTablero.ListarTablerosConTareasAsignadas(idUsuario);
            var esDueño = tablero.idUsuarioPropietario == idUsuario;
            var tieneTareas = tablerosConTareasAsignadas.Any(t => t.id == id);
            if(!esDueño && !tieneTareas){ //Se verifica si, el usuario que no es admin, cumple con los requisitos para ver dicho tablero (Ser dueño o tener tareas asignadas).
                TempData["ErrorMessage"] = "Acceso denegado";
                return RedirectToAction("Index", "Tablero"); //Sino se lo redirecciona al index de tablero, con un mensaje de acceso denegado.
            }
            tableroVM = new VerTableroVM(tableroVM, esDueño, idUsuario); //Si cumple con los requisitos, se le agrega al VM ademas el id del usuario que controla el sistema, y un bool que nos dice si es dueño o no del tablero para establecer acciones para tareas.
        }
        return View(tableroVM);
    }
    [HttpGet]
    public IActionResult AltaTablero(){ //Vista para crear un nuevo tablero, acceso libre.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var model = new CrearTableroVM();
        var idUsuarioPropietario = HttpContext.Session.GetInt32("Id");
        if(idUsuarioPropietario.HasValue){
            model = new CrearTableroVM((int)idUsuarioPropietario); //Se hace verificacion porque dice que puede ser null, pero si el usuario esta logueado no deberia. Se crea un modelo con el id del usuario que controla el sistema para que se coloque como propietario en la vista en un input hidden.
        }
        return View(model);
    }
    [HttpPost]
    public IActionResult CrearTablero(CrearTableroVM tableroVM){ //Método que crea un nuevo tablero.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("AltaTablero");
        var tablero = new Tablero(tableroVM);
        repositorioTablero.CrearTablero(tablero);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarTablero(int id){ //Vista para modificar un tablero dependiendo el id, solo se puede acceder a el si sos dueño del tablero o sos admin.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador";
        ViewData["EsAdmin"] = EsAdmin; 
        var usuarios = repositorioUsuarios.ListarUsuarios();
        var usuariosVM = usuarios.Select(u => new ListarUsuariosVM(u)).ToList();
        var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
        if(tablero.nombre == string.Empty){ //Verificamos si el tablero existe, si no existe se redirecciona a Index.
            TempData["ErrorMessage"] = "El tablero no existe";
            return RedirectToAction("Index", "Tablero");
        }
        var tableroVM = new ModificarTableroVM(tablero, usuariosVM);
        if(!EsAdmin){ //Verificamos si es admin para luego determinar si puede acceder el usuario o no.
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            var esDueño = tablero.idUsuarioPropietario == idUsuario;
            if(!esDueño){ //Se verifica si, el usuario que no es admin, es dueño del tablero al menos.
                TempData["ErrorMessage"] = "Acceso denegado";
                return RedirectToAction("Index", "Tablero"); //Sino se lo redirecciona al index de tablero, con un mensaje de acceso denegado.
            }
        }
        return View(tableroVM);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarTableroVM tableroVM){ //Método que modifica el tablero.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("ModificarTablero", tableroVM.id);
        var tablero = new Tablero(tableroVM);
        repositorioTablero.ModificarTablero(tablero.id, tablero);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarTablero(int id){ //Vista para eliminar un tablero por id. Solo puede acceder el admin o el dueño del tablero y solo si el tablero no tiene tareas cargadas.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        var EsAdmin = HttpContext.Session.GetString("Rol") == "Administrador"; 
        var tablero = repositorioTablero.ObtenerDetallesDeTablero(id);
        if(tablero.nombre == string.Empty){ //Verificamos si el tablero existe, si no existe se redirecciona a Index.
            TempData["ErrorMessage"] = "El tablero no existe";
            return RedirectToAction("Index", "Tablero");
        }
        if(!EsAdmin){ //Verificamos si es admin para luego determinar si puede acceder el usuario o no.
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            var esDueño = tablero.idUsuarioPropietario == idUsuario;
            if(!esDueño){ //Se verifica si, el usuario que no es admin, es dueño del tablero al menos.
                TempData["ErrorMessage"] = "Acceso denegado";
                return RedirectToAction("Index", "Tablero"); //Sino se lo redirecciona al index de tablero, con un mensaje de acceso denegado.
            }
        }
        var tareas = repositorioTareas.ListarTareasDeTablero(id);
        if(tareas.Count>0){ //Verificamos que el tablero no tenga tareas asignadas. Si las tiene, solo se muestra un mensaje de alerta y no deja borrar el tablero.
            ViewData["ErrorMessage"] = "No se puede borrar el tablero porque tiene tareas cargadas.";
        }
        var tableroVM = new EliminarTableroVM(tablero);
        return View(tableroVM);
    }
    [HttpPost]
    public IActionResult Eliminar(EliminarTableroVM tableroVM){ //Metodo para eliminar un tablero por id.
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction ("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("EliminarTablero", tableroVM.id);
        var tablero = new Tablero(tableroVM);
        repositorioTablero.EliminarTableroPorId(tablero.id);
        return RedirectToAction("Index");
    }
}