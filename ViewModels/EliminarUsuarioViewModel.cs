using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class EliminarUsuarioVM{
    private int Id;
    private string NombreDeUsuario;
    public EliminarUsuarioVM(){
        NombreDeUsuario = string.Empty;
    }
    public EliminarUsuarioVM(Usuarios usuario){
        Id = usuario.id;
        NombreDeUsuario = usuario.nombreDeUsuario;
    }
    [Required(ErrorMessage = "ID de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del ID del usuario.
    public int id {get => Id; set => Id = value;}
    [Required(ErrorMessage = "Nombre de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del nombre del usuario.
    public string nombreDeUsuario { get => NombreDeUsuario; set => NombreDeUsuario = value; }
}