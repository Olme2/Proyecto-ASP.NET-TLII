public class Tablero{
    private int Id;
    private int IdUsuarioPropietario;
    private string Nombre;
    private string Descripcion;
    public Tablero(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
    }
    public int id {get => Id; set => Id = value;}
    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
}