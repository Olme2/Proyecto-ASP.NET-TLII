using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ModificarTableroVM{
    private int Id;
    private int IdUsuarioPropietario;
    private string Nombre;
    private string? Descripcion;
    private List<ListarUsuariosVM> ListaDeUsuarios;
    public ModificarTableroVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty; 
        ListaDeUsuarios = new List<ListarUsuariosVM>();
    }
    public ModificarTableroVM(Tablero tablero, List<ListarUsuariosVM> listaDeUsuarios){
        Id = tablero.id;
        IdUsuarioPropietario = tablero.idUsuarioPropietario;
        Nombre = tablero.nombre;
        Descripcion = tablero.descripcion;
        ListaDeUsuarios = listaDeUsuarios;
    }
    [Required(ErrorMessage = "Id de tablero obligatorio.")]
    public int id {get => Id; set => Id = value;}
    [Required(ErrorMessage = "Id de usuario propietario obligatorio.")]
    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}
    [Required(ErrorMessage = "Nombre de tablero obligatorio.")]
    public string nombre {get => Nombre; set => Nombre = value;}
    public string? descripcion {get => Descripcion; set => Descripcion = value;}
    public List<ListarUsuariosVM> listaDeUsuarios {get => ListaDeUsuarios; set => ListaDeUsuarios = value;}
}