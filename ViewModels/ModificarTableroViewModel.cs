using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ModificarTableroVM{
    private int Id;
    private int IdUsuarioPropietario;
    private string Nombre;
    private string Descripcion;
    public ModificarTableroVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty; 
    }
    public ModificarTableroVM(Tablero tablero){
        Id = tablero.id;
        IdUsuarioPropietario = tablero.idUsuarioPropietario;
        Nombre = tablero.nombre;
        Descripcion = tablero.descripcion;
    }
    public int id {get => Id; set => Id = value;}
    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
}