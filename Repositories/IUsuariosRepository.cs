using tl2_proyecto_2024_Olme2.Models;
public interface IUsuariosRepository{
    void CrearUsuario(Usuarios usuario);
    void ModificarUsuario(int id, Usuarios usuario);
    Usuarios ObtenerDetallesDeUsuario(int id);
    Usuarios? ObtenerUsuarioPorNombreYPassword(string nombreDeUsuario, string Password);
    List<Usuarios> ListarUsuarios();
    void CambiarPassword(int id, string password);
    void EliminarUsuarioPorId(int id);
    int BuscarIdPorNombreDeUsuario(string nombreDeUsuario);
}