using System.ComponentModel.DataAnnotations; 
public class ListarUsuariosVM{
    private int Id;
    private string NombreDeUsuario; 
    private Usuarios.Rol RolUsuario;
    public ListarUsuariosVM(){
        NombreDeUsuario = string.Empty;
    }
    public int id {get => Id; set => Id = value;}
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    public Usuarios.Rol rolUsuario {get => RolUsuario; set => RolUsuario = value;}
}