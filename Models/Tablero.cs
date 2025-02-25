namespace tl2_proyecto_2024_Olme2.Models;
public class Tablero{
    private int Id;
    private int IdUsuarioPropietario;
    private string Nombre;
    private string Descripcion;
    public Tablero(){
        Nombre = string.Empty;
        Descripcion = string.Empty;
    }
    public Tablero(int id, int idUsuarioPropietario, string nombre, string descripcion){
        Id = id;
        IdUsuarioPropietario = idUsuarioPropietario;
        Nombre = nombre;
        Descripcion = descripcion;
    }
    public Tablero(CrearTableroVM tableroVM){
        IdUsuarioPropietario = tableroVM.idUsuarioPropietario;
        Nombre = tableroVM.nombre;
        Descripcion = tableroVM.descripcion;
    }
    public Tablero(ListarTablerosVM tableroVM){
        Id= tableroVM.id;
        Nombre = tableroVM.nombre;
        Descripcion = tableroVM.descripcion;
    }
    public Tablero(ModificarTableroVM tableroVM){
        Id = tableroVM.id;
        IdUsuarioPropietario = tableroVM.idUsuarioPropietario;
        Nombre = tableroVM.nombre;
        Descripcion = tableroVM.descripcion;
    }
    public int id {get => Id; set => Id = value;}
    public int idUsuarioPropietario {get => IdUsuarioPropietario; set => IdUsuarioPropietario = value;}
    public string nombre {get => Nombre; set => Nombre = value;}
    public string descripcion {get => Descripcion; set => Descripcion = value;}
}