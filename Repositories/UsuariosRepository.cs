using Microsoft.Data.Sqlite;
using tl2_proyecto_2024_Olme2.Models;
public class UsuariosRepository : IUsuariosRepository{
    private readonly string ConnectionString;
    public UsuariosRepository(string CadenaDeConexion){
        ConnectionString = CadenaDeConexion;
    }
    public void CrearUsuario(Usuarios usuario){ //Método para crear un nuevo usuario.
        string QueryString = @"INSERT INTO Usuario (nombre_de_usuario, password, rolusuario) VALUES (@nombre, @password, @rol);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@nombre", usuario.nombreDeUsuario);
            command.Parameters.AddWithValue("@password", usuario.password);
            command.Parameters.AddWithValue("@rol", usuario.rolUsuario);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void ModificarUsuario(int id, Usuarios usuario){ //Método para modificar un determinado usuario.
        string QueryString = @"UPDATE Usuario SET nombre_de_usuario = @nombre, password = @password, rolusuario = @rol WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@nombre", usuario.nombreDeUsuario);
            command.Parameters.AddWithValue("@password", usuario.password);
            command.Parameters.AddWithValue("@rol", usuario.rolUsuario);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public Usuarios ObtenerDetallesDeUsuario(int id){ //Método para obtener detalles de un determinado usuario.
        Usuarios usuario = new Usuarios();
        string QueryString = @"SELECT nombre_de_usuario, password, rolusuario FROM Usuario WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader = command.ExecuteReader()){
                if(reader.Read()){
                    usuario.id = id;
                    var nombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    var password = reader["password"].ToString();
                    if(nombreDeUsuario!=null){ //El nombre no puede ser nulo, pero igual lo verifico para que no salte error.
                        usuario.nombreDeUsuario = nombreDeUsuario;
                    }
                    if(password!=null){ //La contraseña no puede ser nula, pero igual la verifico para que no salte error.
                        usuario.password = password;
                    }
                    usuario.rolUsuario = (Usuarios.Rol)Convert.ToInt32(reader["rolusuario"]);
                }
            }
            connection.Close();
        }
        return usuario;
    }
    public Usuarios? ObtenerUsuarioPorNombreYPassword(string nombreDeUsuario, string password){//Método para obtener un usuario por inicio de sesion (usando nombre y contraseña).
        Usuarios? usuario = null;
        string QueryString = @"SELECT * FROM Usuario WHERE nombre_de_usuario = @nombreDeUsuario AND password = @password;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@nombreDeUsuario", nombreDeUsuario);
            command.Parameters.AddWithValue("@password", password);
            using(SqliteDataReader reader = command.ExecuteReader()){
                if(reader.Read()){
                    usuario = new Usuarios();
                    usuario.id = Convert.ToInt32(reader["id"]);
                    usuario.nombreDeUsuario = nombreDeUsuario;
                    usuario.password = password;
                    usuario.rolUsuario = (Usuarios.Rol)Convert.ToInt32(reader["rolusuario"]);
                }
            }
            connection.Close();
        }
        return usuario;
    }
    public List<Usuarios> ListarUsuarios(){ //Método para listar todos los usuarios.
        List<Usuarios> listaDeUsuarios = new List<Usuarios>();
        string QueryString = @"SELECT * FROM Usuario;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    Usuarios usuario = new Usuarios();
                    usuario.id = Convert.ToInt32(reader["id"]);
                    var nombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    var password = reader["password"].ToString();
                    if(nombreDeUsuario!=null){ //El nombre no puede ser nulo, pero igual lo verifico para que no salte error.
                        usuario.nombreDeUsuario = nombreDeUsuario;
                    }
                    if(password!=null){ //La contraseña no puede ser nula, pero igual la verifico para que no salte error.
                        usuario.password = password;
                    }
                    usuario.rolUsuario = (Usuarios.Rol)Convert.ToInt32(reader["rolusuario"]);
                    listaDeUsuarios.Add(usuario);
                }
            }
            connection.Close();
        }
        return listaDeUsuarios;
    }
    public void CambiarPassword(int id, string password){ //Método para cambiar la contraseña de un determinado usuario por una nueva.
        string QueryString = @"UPDATE Usuario SET password = @password WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@password", password);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void EliminarUsuarioPorId(int id){ //Método para eliminar un determinado usuario.
        string QueryString = @"DELETE FROM Usuario WHERE id = @id AND id NOT IN (SELECT id_usuario_propietario FROM Tablero WHERE id_usuario_propietario = @id) AND id NOT IN (SELECT id_usuario_asignado FROM Tarea WHERE id_usuario_asignado = @id);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public int BuscarIdPorNombreDeUsuario(string nombreDeUsuario){ //Método para buscar el id de un usuario usando su nombre, ya que el nombre es único. Sirve para modificar la contraseña.
        int id = -1; //Si no encuentra el id, devuelve -1.
        string QueryString = @"SELECT id FROM Usuario WHERE nombre_de_usuario = @nombreDeUsuario;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@nombreDeUsuario", nombreDeUsuario);
            using(SqliteDataReader reader = command.ExecuteReader()){
                if(reader.Read()){
                    id = Convert.ToInt32(reader["id"]);
                }
            }
            connection.Close();
        }
        return id;
    }
}