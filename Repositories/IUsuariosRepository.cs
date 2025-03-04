using tl2_proyecto_2024_Olme2.Models;
public interface IUsuariosRepository{
    void CrearUsuario(Usuarios usuario); //Método para crear un nuevo usuario.
    void ModificarUsuario(int id, Usuarios usuario); //Método para modificar un determinado usuario.
    Usuarios ObtenerDetallesDeUsuario(int id); //Método para obtener detalles de un determinado usuario.
    Usuarios? ObtenerUsuarioPorNombreYPassword(string nombreDeUsuario, string Password); //Método para obtener un usuario por inicio de sesion (usando nombre y contraseña).
    List<Usuarios> ListarUsuarios(); //Método para listar todos los usuarios.
    void CambiarPassword(int id, string password); //Método para cambiar la contraseña de un determinado usuario por una nueva.
    void EliminarUsuarioPorId(int id); //Método para eliminar un determinado usuario.
}