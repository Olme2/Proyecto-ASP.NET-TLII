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
    public ModificarTareaVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Color = string.Empty;
    }
    public ModificarTareaVM(Tareas tarea){
        Id = tarea.id;
        IdTablero = tarea.idTablero;
        Nombre = tarea.nombre;
        Descripcion = tarea.descripcion;
        Color = tarea.color;
        Estado = tarea.estado;
        IdUsuarioAsignado = tarea.idUsuarioAsignado;
    }
    public int id {get => Id; set => Id = value;}
    public int idTablero {get => IdTablero; set => IdTablero = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string? descripcion {get => Descripcion; set => Descripcion = value;}
    public string? color {get => Color; set => Color = value;}
    public Tareas.EstadoTarea estado {get => Estado; set => Estado = value;}
    public int? idUsuarioAsignado {get => IdUsuarioAsignado; set => IdUsuarioAsignado = value;}
}