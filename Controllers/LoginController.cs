using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller{
    private readonly ILogger<LoginController> _logger;
    private IUsuariosRepository repositorioUsuarios;
    public LoginController(ILogger<LoginController> logger, IUsuariosRepository RepositorioUsuarios){
        _logger = logger;
        repositorioUsuarios = RepositorioUsuarios;
    }
}