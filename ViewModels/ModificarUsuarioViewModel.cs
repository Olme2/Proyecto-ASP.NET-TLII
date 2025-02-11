using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ModificarUsuarioVM{
    private int Id;
    private string NombreDeUsuario; 
    private string Password;
    private Usuarios.Rol RolUsuario;
    public ModificarUsuarioVM(){
        NombreDeUsuario = string.Empty;
        Password = string.Empty;
    }
    public ModificarUsuarioVM(Usuarios usuario){
        Id = usuario.id;
        NombreDeUsuario = usuario.nombreDeUsuario;
        Password = usuario.password;
        RolUsuario = usuario.rolUsuario;
    }
    public int id {get => Id; set => Id = value;}
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    public string password {get => Password; set => Password = value;}
    public Usuarios.Rol rolUsuario {get => RolUsuario; set => RolUsuario = value;}
}