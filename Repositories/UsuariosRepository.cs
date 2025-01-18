using Microsoft.Data.Sqlite;
public class UsuariosRepository : IUsuariosRepository{
    private readonly string ConnectionString;
    public UsuariosRepository(string CadenaDeConexion){
        ConnectionString = CadenaDeConexion;
    }
    public void CrearUsuario(Usuarios usuario){
        string QueryString = @"INSERT INTO Usuario VALUES (@id, @nombre, @password, @rol);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", usuario.id);
            command.Parameters.AddWithValue("@nombre", usuario.nombreDeUsuario);
            command.Parameters.AddWithValue("@password", usuario.password);
            command.Parameters.AddWithValue("@rol", usuario.rolUsuario);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void ModificarUsuario(int id, Usuarios usuario){
        string QueryString = @"UPDATE Usuario SET nombre_de_usuario = @nombre, password = @password, rolusuario = @rol WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", usuario.id);
            command.Parameters.AddWithValue("@nombre", usuario.nombreDeUsuario);
            command.Parameters.AddWithValue("@password", usuario.password);
            command.Parameters.AddWithValue("@rol", usuario.rolUsuario);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public Usuarios ObtenerDetallesDeUsuario(int id){
        Usuarios usuario = new Usuarios();
        string QueryString = @"SELECT nombre_de_usuario, password, rolusuario FROM Usuario WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader = command.ExecuteReader()){
                if(reader.Read()){
                    var nombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    var password = reader["password"].ToString();
                    if(nombreDeUsuario!=null){
                        usuario.nombreDeUsuario = nombreDeUsuario;
                    }
                    if(password!=null){
                        usuario.password = password;
                    }
                    usuario.rolUsuario = (Usuarios.Rol)Convert.ToInt32(reader["rolusuario"]);
                }
            }
            connection.Close();
        }
        return usuario;
    }
    public List<Usuarios> ListarUsuarios(){
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
                    if(nombreDeUsuario!=null){
                        usuario.nombreDeUsuario = nombreDeUsuario;
                    }
                    if(password!=null){
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
    public void CambiarPassword(int id, string password){
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
    public void EliminarUsuarioPorId(int id){
        string QueryString = @"DELETE FROM Usuario WHERE id = @id AND id NOT IN (SELECT id_usuario_propietario FROM Tablero WHERE id_usuario_propietario = @id) AND id NOT IN (SELECT id_usuario_asignado FROM Tarea WHERE id_usuario_asignado = @id);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}