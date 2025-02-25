namespace tl2_proyecto_2024_Olme2.Models;
public class Tareas{
    private int Id;
    private int IdTablero;
    private string Nombre;
    private string Descripcion;
    private string Color;
    private EstadoTarea Estado;
    private int? IdUsuarioAsignado;
    public Tareas(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Color = string.Empty;
    }
    public Tareas(int id, int idTablero, string nombre, string descripcion, string color, EstadoTarea estado){
        Id = id;
        IdTablero = idTablero;
        Nombre = nombre;
        Descripcion = descripcion;
        Color = color; 
        Estado = estado;
    }
    public Tareas(CrearTareaVM tareaVM){
        IdTablero = tareaVM.idTablero;
        Nombre = tareaVM.nombre;
        Descripcion = tareaVM.descripcion;
        Color = tareaVM.color; 
        Estado = tareaVM.estado;
    }
    public Tareas(ListarTareasVM tareaVM){
        Id = tareaVM.id;
        Nombre = tareaVM.nombre;
        Descripcion = tareaVM.descripcion;
        Color = tareaVM.color;
        Estado = tareaVM.estado;
    }
    public Tareas(ModificarTareaVM tareaVM){
        Id = tareaVM.id;
        IdTablero = tareaVM.idTablero;
        Nombre = tareaVM.nombre;
        Descripcion = tareaVM.descripcion;
        Color = tareaVM.color;
        Estado = tareaVM.estado;
        IdUsuarioAsignado= tareaVM.idUsuarioAsignado;
    }
    public int id {get => Id; set => Id = value;}
    public int idTablero {get => IdTablero; set => IdTablero = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
    public string color {get => Color; set => Color = value;}
    public EstadoTarea estado {get => Estado; set => Estado = value;}
    public int? idUsuarioAsignado {get => IdUsuarioAsignado; set => IdUsuarioAsignado = value;}
    public enum EstadoTarea{
        Ideas = 1,
        ToDo = 2,
        Doing = 3,
        Review = 4,
        Done = 5
    }
}