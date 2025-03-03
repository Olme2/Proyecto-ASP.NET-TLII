using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class CrearUsuarioVM{
    private string NombreDeUsuario;
    private string Password;
    private Usuarios.Rol RolUsuario;
    public CrearUsuarioVM(){
        NombreDeUsuario = string.Empty;
        Password = string.Empty;
    }
    [Required(ErrorMessage = "Nombre de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del nombre de usuario.
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    [Required(ErrorMessage = "Contraseña obligatoria.")] //Validacion en backend para la obligatoriedad de la contraseña.
    public string password {get => Password; set => Password = value;}
    [Required(ErrorMessage = "Rol de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del rol de usuario.
    public Usuarios.Rol rolUsuario {get => RolUsuario; set => RolUsuario = value;}
}