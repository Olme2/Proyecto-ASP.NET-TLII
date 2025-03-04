using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ListarUsuariosVM{

    private int Id;
    private string NombreDeUsuario; 

    public ListarUsuariosVM(){
        NombreDeUsuario = string.Empty;
    }

    public ListarUsuariosVM(Usuarios usuario){
        Id = usuario.id;
        NombreDeUsuario = usuario.nombreDeUsuario;
    }

    public int id {get => Id; set => Id = value;}

    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}

}