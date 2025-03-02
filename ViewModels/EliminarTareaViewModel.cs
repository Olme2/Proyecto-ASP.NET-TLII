using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class EliminarTareaVM{
    private int Id;
    private int IdTablero;
    private string Nombre;
    public EliminarTareaVM(){
        Nombre = string.Empty;
    }
    public EliminarTareaVM(Tareas tarea){
        Id = tarea.id;
        IdTablero = tarea.idTablero;
        Nombre = tarea.nombre;
    }
    [Required(ErrorMessage = "ID de tarea obligatorio.")] //Validacion en backend para la obligatoriedad del ID de la tarea.
    public int id {get => Id; set => Id = value;}
    [Required(ErrorMessage = "ID de tablero obligatorio.")] // Validacion en backend para la obligatoriedad del ID del tablero.
    public int idTablero {get => IdTablero; set => IdTablero = value;}
    [Required(ErrorMessage = "Nombre de tarea obligatorio.")] //Validacion en backend para la obligatoriedad del nombre de la tarea.
    public string nombre { get => Nombre; set => Nombre = value; }
}