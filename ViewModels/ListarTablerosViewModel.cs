using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class ListarTablerosVM{
    private int Id;
    private string Nombre;
    private string Descripcion;
    public ListarTablerosVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
    }
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
    public int id {get => Id; set => Id = value;}
}