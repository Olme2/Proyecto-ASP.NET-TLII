using System.ComponentModel.DataAnnotations; 
public class LoginVM{
    private string NombreDeUsuario;
    private string Password;
    public LoginVM(){
        NombreDeUsuario = string.Empty;
        Password = string.Empty;
    }
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    public string password {get => Password; set => Password = value;}
}