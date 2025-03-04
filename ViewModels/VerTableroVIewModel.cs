using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class VerTableroVM{

    private int Id;
    private bool EsDueño;
    private int? IdUsuarioVisitante;
    private string NombreUsuarioPropietario;
    private string Nombre;
    private string? Descripcion;
    private List<ListarTareasVM> ListaDeTareas;

    public VerTableroVM(){
        NombreUsuarioPropietario = string.Empty;
        Nombre = string.Empty;
        Descripcion = string.Empty;
        ListaDeTareas = new List<ListarTareasVM>();
    }

    public VerTableroVM(Tablero tablero, List<ListarTareasVM> listaDeTareas, string nombreUsuarioPropietario){
        Id = tablero.id;
        EsDueño = true;
        NombreUsuarioPropietario = nombreUsuarioPropietario;
        Nombre = tablero.nombre;
        Descripcion = tablero.descripcion;
        ListaDeTareas = listaDeTareas;
    }

    public VerTableroVM(VerTableroVM tablero, bool esDueño, int idUsuarioVisitante){
        Id = tablero.Id;
        IdUsuarioVisitante = idUsuarioVisitante;
        EsDueño = esDueño;
        NombreUsuarioPropietario = tablero.nombreUsuarioPropietario;
        Nombre = tablero.nombre;
        Descripcion = tablero.descripcion;
        ListaDeTareas = tablero.listaDeTareas;
    }

    public int id {get => Id; set => Id = value;}
    public bool esDueño {get => EsDueño; set => EsDueño = value;}
    public int? idUsuarioVisitante {get => IdUsuarioVisitante; set => IdUsuarioVisitante = value;}
    public string nombreUsuarioPropietario {get => NombreUsuarioPropietario; set => NombreUsuarioPropietario = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string? descripcion {get => Descripcion; set => Descripcion = value;}
    public List<ListarTareasVM> listaDeTareas {get => ListaDeTareas; set => ListaDeTareas = value;}

}