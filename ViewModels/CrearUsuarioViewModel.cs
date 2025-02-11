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
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    public string password {get => Password; set => Password = value;}
    public Usuarios.Rol rolUsuario {get => RolUsuario; set => RolUsuario = value;}
}