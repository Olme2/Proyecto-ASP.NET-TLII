using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class CambiarPasswordVM{
    
    private int Id;
    private string NombreDeUsuario;
    private string Password;
    private string ConfirmarPassword;
    
    public CambiarPasswordVM(){
        NombreDeUsuario = string.Empty;
        Password = string.Empty;
        ConfirmarPassword = string.Empty;
    }
    
    public CambiarPasswordVM(Usuarios usuario){
        Id = usuario.id;
        NombreDeUsuario = usuario.nombreDeUsuario;
        Password = string.Empty;
        ConfirmarPassword = string.Empty;
    }
    
    public int id {get => Id; set => Id = value;}
    
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    
    [Required(ErrorMessage = "Contraseña obligatoria.")] //Validacion en backend para la obligatoriedad de la contraseña.
    public string password {get => Password; set => Password = value;}
    
    [Required(ErrorMessage = "Repita su contraseña por favor.")] //Validacion en backend para la obligatoriedad de la contraseña repetida.
    [Compare("password", ErrorMessage = "Las contraseñas no coinciden. Inténtelo de nuevo.")] //Validacion en backend para la obligatoriedad de la coincidencia entre contraseña y contraseña repetida.
    public string confirmarPassword {get => ConfirmarPassword; set => ConfirmarPassword = value;}

}