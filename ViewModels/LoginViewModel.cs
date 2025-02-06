using System.ComponentModel.DataAnnotations; 
public class LoginVM{
    private string NombreDeUsuario;
    private string Password;
    private string Error;
    private bool Autenticado;
    public LoginVM(){
        NombreDeUsuario = string.Empty;
        Password = string.Empty;
        Error = string.Empty;
    }
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    public string password {get => Password; set => Password = value;}
    public string error {get => Error; set => Error = value;}
    public bool autenticado {get => Autenticado; set => Autenticado = value;}
}