using System.ComponentModel.DataAnnotations; 
public class CrearTableroVM{
    private string Nombre;
    private string Descripcion;
    public CrearTableroVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
    }
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
}