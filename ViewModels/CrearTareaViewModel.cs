using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class CrearTareaVM{
    private int IdTablero;
    private string Nombre;
    private string Descripcion;
    private string Color;
    private Tareas.EstadoTarea Estado;
    public CrearTareaVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Color = string.Empty;
    }
    public int idTablero {get => IdTablero; set => IdTablero = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
    public string color {get => Color; set => Color = value;}
    public Tareas.EstadoTarea estado {get => Estado; set => Estado = value;}
}