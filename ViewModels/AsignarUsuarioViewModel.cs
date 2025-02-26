using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class AsignarUsuarioVM{
    private int IdTarea;
    private int IdTablero;
    private int? IdUsuario;
    private List<Usuarios> ListaDeUsuarios;
    public AsignarUsuarioVM(){
        ListaDeUsuarios = new List<Usuarios>();
    }
    public AsignarUsuarioVM(Tareas tarea){
        IdTarea = tarea.id;
        IdTablero = tarea.idTablero;
        IdUsuario = tarea.idUsuarioAsignado;
        ListaDeUsuarios = new List<Usuarios>();
    }
    public int idTarea {get => IdTarea; set => IdTarea = value;}
    public int idTablero {get => IdTablero; set => IdTablero = value;}
    public int? idUsuario {get => IdUsuario; set => IdUsuario = value;}
    public List<Usuarios> listaDeUsuarios {get => ListaDeUsuarios; set => ListaDeUsuarios = value;}
}