using System.ComponentModel.DataAnnotations; 
public class ModificarUsuarioVM{
    private string NombreDeUsuario; 
    private string Password;
    private Usuarios.Rol RolUsuario;
    public ModificarUsuarioVM(){
        NombreDeUsuario = string.Empty;
        Password = string.Empty;
    }
    public ModificarUsuarioVM(Usuarios usuario){
        NombreDeUsuario = usuario.nombreDeUsuario;
        Password = usuario.password;
        RolUsuario = usuario.rolUsuario;
    }
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    public string password {get => Password; set => Password = value;}
    public Usuarios.Rol rolUsuario {get => RolUsuario; set => RolUsuario = value;}
}