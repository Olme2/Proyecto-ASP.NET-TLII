using System.ComponentModel.DataAnnotations; 
public class ModificarTableroVM{
    private int IdUsuarioPropietario;
    private string Nombre;
    private string Descripcion;
    public ModificarTableroVM(){
        Nombre = string.Empty;
        Descripcion = string.Empty; 
    }
    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
}