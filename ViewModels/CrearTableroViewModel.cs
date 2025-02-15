using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class CrearTableroVM{
    private int IdUsuarioPropietario;
    private string Nombre;
    private string Descripcion;
    public CrearTableroVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
    }
    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
}