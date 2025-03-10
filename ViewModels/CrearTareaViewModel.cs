using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class CrearTareaVM{
    
    private int IdTablero;
    private string Nombre;
    private string? Descripcion;
    private string? Color;
    private Tareas.EstadoTarea Estado;
    private List<ListarTablerosVM> ListaDeTableros;
    
    public CrearTareaVM(){
        Nombre = string.Empty;
        ListaDeTableros = new List<ListarTablerosVM>();
    }
    
    public CrearTareaVM(int idTablero){
        Nombre = string.Empty;
        IdTablero = idTablero;
        ListaDeTableros = new List<ListarTablerosVM>();
    }

    public CrearTareaVM(int idTablero, List<ListarTablerosVM> listaDeTableros){
        Nombre = string.Empty;
        IdTablero = idTablero;
        ListaDeTableros = listaDeTableros;
    }
    
    [Required(ErrorMessage = "ID de tablero obligatorio.")] //Validacion en backend para la obligatoriedad del ID del tablero.
    public int idTablero {get => IdTablero; set => IdTablero = value;}
    
    [Required(ErrorMessage = "Nombre de tarea obligatorio.")] //Validacion en backend para la obligatoriedad del nombre de la tarea.
    public string nombre {get => Nombre; set => Nombre = value;}
    
    public string? descripcion {get => Descripcion; set => Descripcion = value;}
    
    public string? color {get => Color; set => Color = value;}
    
    [Required(ErrorMessage = "Estado de tarea obligatorio.")] //Validacion en backend para la obligatoriedad del estado de la tarea.
    public Tareas.EstadoTarea estado {get => Estado; set => Estado = value;}

    public List<ListarTablerosVM> listaDeTableros {get => ListaDeTableros; set => ListaDeTableros = value;}

}