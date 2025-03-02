using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class AsignarUsuarioVM{
    private int IdTarea;
    private int IdTablero;
    private int? IdUsuario;
    private string Nombre;
    private List<ListarUsuariosVM> ListaDeUsuarios;
    public AsignarUsuarioVM(){
        Nombre = string.Empty;
        ListaDeUsuarios = new List<ListarUsuariosVM>();
    }
    public AsignarUsuarioVM(Tareas tarea, List<ListarUsuariosVM> listaDeUsuarios){
        IdTarea = tarea.id;
        IdTablero = tarea.idTablero;
        IdUsuario = tarea.idUsuarioAsignado;
        Nombre = tarea.nombre;
        ListaDeUsuarios = listaDeUsuarios;
    }
    [Required(ErrorMessage = "ID de tarea obligatorio.")]
    public int idTarea {get => IdTarea; set => IdTarea = value;}
    [Required(ErrorMessage = "ID de tablero obligatorio.")]
    public int idTablero {get => IdTablero; set => IdTablero = value;}
    public int? idUsuario {get => IdUsuario; set => IdUsuario = value;}
    [Required(ErrorMessage = "Nombre de tarea obligatorio.")]
    public string nombre {get => Nombre; set => Nombre = value;}
    public List<ListarUsuariosVM> listaDeUsuarios {get => ListaDeUsuarios; set => ListaDeUsuarios = value;}
}