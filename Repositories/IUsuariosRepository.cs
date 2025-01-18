public interface IUsuariosRepository{
    void CrearUsuario(Usuarios usuario);
    void ModificarUsuario(int id, Usuarios usuario);
    Usuarios ObtenerDetallesDeUsuario(int id);
    List<Usuarios> ListarUsuarios();
    void CambiarPassword(int id, string password);
    void EliminarUsuarioPorId(int id);
}