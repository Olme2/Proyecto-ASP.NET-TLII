using System.ComponentModel.DataAnnotations; 
public class ListarTareasVM{
    private int Id;
    private string Nombre;
    private string Descripcion;
    private string Color;
    private Tareas.EstadoTarea Estado;
    public ListarTareasVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Color = string.Empty;
    }
    public int id {get => Id; set => Id = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
    public string color {get => Color; set => Color = value;}
    public Tareas.EstadoTarea estado {get => Estado; set => Estado = value;}
}