using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ListarTablerosVM{
    
    private int Id;
    private int IdUsuarioPropietario;
    private string Nombre;
    private string? Descripcion;
    private string NombreUsuarioPropietario;
    
    public ListarTablerosVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
        NombreUsuarioPropietario = string.Empty;
    }
    
    public ListarTablerosVM(Tablero tablero){
        Id = tablero.id;
        Nombre = tablero.nombre;
        Descripcion = string.Empty;
        NombreUsuarioPropietario = string.Empty;
    }

    public ListarTablerosVM(Tablero tablero, string nombreUsuarioPropietario){
        Id = tablero.id;
        IdUsuarioPropietario = tablero.idUsuarioPropietario;
        Nombre = tablero.nombre;
        Descripcion = tablero.descripcion;
        NombreUsuarioPropietario = nombreUsuarioPropietario;
    }
    
    public int id {get => Id; set => Id = value;}

    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}

    public string nombre {get => Nombre; set => Nombre = value;}

    public string? descripcion {get => Descripcion; set => Descripcion = value;}

    public string nombreUsuarioPropietario {get => NombreUsuarioPropietario; set => nombreUsuarioPropietario = value;}

}