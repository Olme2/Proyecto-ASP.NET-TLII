using System.ComponentModel.DataAnnotations; 
namespace tl2_proyecto_2024_Olme2.Models;
public class LoginVM{
    private int Id;
    private string NombreDeUsuario;
    private string Password;
    private string Error;
    private bool Autenticado;
    public LoginVM(){
        NombreDeUsuario = string.Empty;
        Password = string.Empty;
        Error = string.Empty;
    }
    public int id {get => Id; set => Id = value;}
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    public string password {get => Password; set => Password = value;}
    public string error {get => Error; set => Error = value;}
    public bool autenticado {get => Autenticado; set => Autenticado = value;}
}