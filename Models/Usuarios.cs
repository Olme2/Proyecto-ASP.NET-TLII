namespace tl2_proyecto_2024_Olme2.Models;
public class Usuarios{
    private int Id;
    private string NombreDeUsuario; 
    private string Password;
    private Rol RolUsuario;
    public Usuarios(){
        NombreDeUsuario = string.Empty;
        Password = string.Empty;
    }
    public Usuarios(int id, string nombreDeUsuario, string password, Rol rolUsuario){
        Id = id;
        NombreDeUsuario = nombreDeUsuario;
        Password = password;
        RolUsuario = rolUsuario;
    }
    public Usuarios(CrearUsuarioVM usuarioVM){
        NombreDeUsuario = usuarioVM.nombreDeUsuario;
        Password = usuarioVM.password;
        RolUsuario = usuarioVM.rolUsuario;
    }
    public Usuarios(ModificarUsuarioVM usuarioVM){
        Id = usuarioVM.id;
        NombreDeUsuario = usuarioVM.nombreDeUsuario;
        Password = usuarioVM.password;
        RolUsuario = usuarioVM.rolUsuario;
    }
    public Usuarios(CambiarPasswordVM usuarioVM){
        Id = usuarioVM.id;
        NombreDeUsuario = usuarioVM.nombreDeUsuario;
        Password = usuarioVM.password;
    }
    public int id {get => Id; set => Id = value;}
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    public string password {get => Password; set => Password = value;}
    public Rol rolUsuario {get => RolUsuario; set => RolUsuario = value;}
    public enum Rol{
        Administrador = 1,
        Operador = 2
    }
}