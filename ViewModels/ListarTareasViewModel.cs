using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ListarTareasVM{
    private int Id;
    private string Nombre;
    private string? Descripcion;
    private string? Color;
    private Tareas.EstadoTarea Estado;
    private int? IdUsuarioAsignado;
    public ListarTareasVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Color = string.Empty;
    }
    public ListarTareasVM(Tareas tarea){
        Id = tarea.id;
        Nombre = tarea.nombre;
        Descripcion = tarea.descripcion;
        Color = tarea.color;
        Estado = tarea.estado;
        IdUsuarioAsignado = tarea.idUsuarioAsignado;
    }
    public int id {get => Id; set => Id = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string? descripcion {get => Descripcion; set => Descripcion = value;}
    public string? color {get => Color; set => Color = value;}
    public Tareas.EstadoTarea estado {get => Estado; set => Estado = value;}
    public int? idUsuarioAsignado {get => IdUsuarioAsignado; set => IdUsuarioAsignado = value;}
}