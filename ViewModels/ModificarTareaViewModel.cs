using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ModificarTareaVM{

    private int Id;
    private int IdTablero;
    private string Nombre;
    private string? Descripcion;
    private string? Color;
    private Tareas.EstadoTarea Estado;
    private int? IdUsuarioAsignado;
    private bool EsDueño;
    private List<ListarTablerosVM> ListaDeTableros;

    public ModificarTareaVM(){
        Nombre = string.Empty;
        ListaDeTableros = new List<ListarTablerosVM>();
    }

    public ModificarTareaVM(Tareas tarea){
        Id = tarea.id;
        IdTablero = tarea.idTablero;
        Nombre = tarea.nombre;
        Descripcion = tarea.descripcion;
        Color = tarea.color;
        Estado = tarea.estado;
        IdUsuarioAsignado = tarea.idUsuarioAsignado;
        EsDueño = true;
        ListaDeTableros = new List<ListarTablerosVM>();
    }

    public ModificarTareaVM(ModificarTareaVM tarea, bool esDueño){
        Id = tarea.id;
        IdTablero = tarea.idTablero;
        Nombre = tarea.nombre;
        Descripcion = tarea.descripcion;
        Color = tarea.color;
        Estado = tarea.estado;
        IdUsuarioAsignado = tarea.idUsuarioAsignado;
        EsDueño = esDueño;
        ListaDeTableros = new List<ListarTablerosVM>();
    }

    public ModificarTareaVM(ModificarTareaVM tarea, List<ListarTablerosVM> listaDeTableros){
        Id = tarea.id;
        IdTablero = tarea.idTablero;
        Nombre = tarea.nombre;
        Descripcion = tarea.descripcion;
        Color = tarea.color;
        Estado = tarea.estado;
        IdUsuarioAsignado = tarea.idUsuarioAsignado;
        EsDueño = true;
        ListaDeTableros = listaDeTableros;
    }

    [Required(ErrorMessage = "ID de tarea obligatorio.")]  //Validacion en backend para la obligatoriedad del ID de la tarea.
    public int id {get => Id; set => Id = value;}

    [Required(ErrorMessage = "ID de tablero obligatorio.")]  //Validacion en backend para la obligatoriedad del ID del tablero.
    public int idTablero {get => IdTablero; set => IdTablero = value;}

    [Required(ErrorMessage = "Nombre de tarea obligatorio.")]  //Validacion en backend para la obligatoriedad del nombre de la tarea.
    public string nombre {get => Nombre; set => Nombre = value;}

    public string? descripcion {get => Descripcion; set => Descripcion = value;}

    public string? color {get => Color; set => Color = value;}

    [Required(ErrorMessage = "Estado de tarea obligatorio.")]  //Validacion en backend para la obligatoriedad del estado de la tarea.
    public Tareas.EstadoTarea estado {get => Estado; set => Estado = value;}

    public int? idUsuarioAsignado {get => IdUsuarioAsignado; set => IdUsuarioAsignado = value;}

    public bool esDueño {get => EsDueño; set => EsDueño = value;}

    public List<ListarTablerosVM> listaDeTableros {get => ListaDeTableros; set => ListaDeTableros = value;}

}