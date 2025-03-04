using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
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
    
    [Required(ErrorMessage = "ID de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del ID del usuario.
    public int id {get => Id; set => Id = value;}
    
    [Required(ErrorMessage = "Nombre de usuario obligatorio.")] //Validacion en backend para la obligatoriedad del nombre de usuario.
    public string nombreDeUsuario {get => NombreDeUsuario; set => NombreDeUsuario = value;}
    
    [Required(ErrorMessage = "Contraseña obligatoria.")] //Validacion en backend para la obligatoriedad de la contraseña.
    public string password {get => Password; set => Password = value;}
    
    public string error {get => Error; set => Error = value;}
    
    public bool autenticado {get => Autenticado; set => Autenticado = value;}

}