using System.ComponentModel.DataAnnotations; 
public class CrearTareaVM{
    private string Nombre;
    private string Descripcion;
    private string Color;
    private Tareas.EstadoTarea Estado;
    public CrearTareaVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Color = string.Empty;
    }
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
    public string color {get => Color; set => Color = value;}
    public Tareas.EstadoTarea estado {get => Estado; set => Estado = value;}
}