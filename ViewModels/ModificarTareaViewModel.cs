using System.ComponentModel.DataAnnotations; 
public class ModificarTareaVM{
    private int IdTablero;
    private string Nombre;
    private string Descripcion;
    private string Color;
    private Tareas.EstadoTarea Estado;
    private int? IdUsuarioAsignado;
    public ModificarTareaVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Color = string.Empty;
    }
    public ModificarTareaVM(Tareas tarea){
        IdTablero = tarea.idTablero;
        Nombre = tarea.nombre;
        Descripcion = tarea.descripcion;
        Color = tarea.color;
        Estado = tarea.estado;
    }
    public int idTablero {get => IdTablero; set => IdTablero = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
    public string color {get => Color; set => Color = value;}
    public Tareas.EstadoTarea estado {get => Estado; set => Estado = value;}
    public int? idUsuarioAsignado {get => IdUsuarioAsignado; set => IdUsuarioAsignado = value;}
}