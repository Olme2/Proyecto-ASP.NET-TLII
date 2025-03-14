using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class CrearTableroVM{
    
    private int IdUsuarioPropietario;
    private string Nombre;
    private string? Descripcion;
    private List<ListarUsuariosVM> ListaDeUsuarios;
    
    public CrearTableroVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        ListaDeUsuarios = new List<ListarUsuariosVM>();
    }
    
    public CrearTableroVM(int idUsuarioPropietario){
        IdUsuarioPropietario = idUsuarioPropietario;
        Nombre = string.Empty;
        Descripcion = string.Empty;
        ListaDeUsuarios = new List<ListarUsuariosVM>();
    }

    public CrearTableroVM(List<ListarUsuariosVM> listaDeUsuarios){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        ListaDeUsuarios = listaDeUsuarios;
    }
    
    [Required(ErrorMessage = "ID de usuario propietario obligatorio.")] //Validacion en backend para la obligatoriedad del ID del usuario propietario.
    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}
    
    [Required(ErrorMessage = "Nombre del tablero obligatorio.")] //Validacion en backend para la obligatoriedad del nombre del tablero.
    public string nombre {get => Nombre; set => Nombre = value;}
    
    public string? descripcion {get => Descripcion; set => Descripcion = value;}

    public List<ListarUsuariosVM> listaDeUsuarios {get => ListaDeUsuarios; set => ListaDeUsuarios = value;}

}