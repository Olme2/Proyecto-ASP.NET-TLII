using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ModificarUsuarioVM{

    private int Id;
    private string NombreDeUsuario;     private Usuarios.Rol RolUsuario;

    public ModificarUsuarioVM(){
        NombreDeUsuario = string.Empty;
    }

    public ModificarUsuarioVM(Usuarios usuario){
        Id = usuario.id;
        NombreDeUsuario = usuario.nombreDeUsuario;
        RolUsuario = usuario.rolUsuario;
    }

    [Required(ErrorMessage = "ID de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del ID del usuario.
    public int id {get => Id; set => Id = value;}

    [Required(ErrorMessage = "Nombre de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del nombre del usuario.
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}

    [Required(ErrorMessage = "Rol de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del rol del usuario.
    public Usuarios.Rol rolUsuario {get => RolUsuario; set => RolUsuario = value;}

}