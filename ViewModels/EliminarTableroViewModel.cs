using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class EliminarTableroVM{
    
    private int Id;
    private string Nombre;
    
    public EliminarTableroVM(){
        Nombre = string.Empty;
    }
    
    public EliminarTableroVM(Tablero tablero){
        Id = tablero.id;
        Nombre = tablero.nombre;
    }
    
    [Required(ErrorMessage = "Id de tablero obligatorio.")] // Validacion en backend para la obligatoriedad del ID del tablero.
    public int id {get => Id; set => Id = value;}
    
    [Required(ErrorMessage = "Nombre de tablero obligatorio.")] //Validacion en backend para la obligatoriedad del nombre del tablero.
    public string nombre { get => Nombre; set => Nombre = value; }

}