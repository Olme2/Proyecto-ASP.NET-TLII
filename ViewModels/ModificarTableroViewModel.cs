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
    [Required(ErrorMessage = "ID de tablero obligatorio.")] //Validacion en backend para la obligatoriedad del ID del tablero.
    public int id {get => Id; set => Id = value;}
    [Required(ErrorMessage = "ID de usuario propietario obligatorio.")] //Validacion en backend para la obligatoriedad del ID del usuario propietario.
    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}
    [Required(ErrorMessage = "Nombre de tablero obligatorio.")] //Validacion en backend para la obligatoriedad del nombre del tablero.
    public string nombre {get => Nombre; set => Nombre = value;}
    public string? descripcion {get => Descripcion; set => Descripcion = value;}
    public List<ListarUsuariosVM> listaDeUsuarios {get => ListaDeUsuarios; set => ListaDeUsuarios = value;}
}