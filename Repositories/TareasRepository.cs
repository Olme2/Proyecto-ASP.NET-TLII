using Microsoft.Data.Sqlite;
public class TareasRepository : ITareasRepository{
    private readonly string ConnectionString;
    public TareasRepository(string cadenaDeConexion){
        ConnectionString = cadenaDeConexion;
    }
    public void CrearTarea(int idTablero, Tareas tarea){
        string QueryString = @"INSERT INTO Tarea VALUES(@id, @idTablero, @nombre, @estado, @descripcion, @color, @idUsuario);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", tarea.id);
            command.Parameters.AddWithValue("@idTablero", tarea.idTablero);
            command.Parameters.AddWithValue("@nombre", tarea.nombre);
            command.Parameters.AddWithValue("@estado", tarea.estado);
            command.Parameters.AddWithValue("@descripcion", tarea.descripcion);
            command.Parameters.AddWithValue("@color", tarea.color);
            command.Parameters.AddWithValue("@idUsuario", tarea.idUsuarioAsignado);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void ModificarTarea(int id, Tareas tarea){
        string QueryString = @"UPDATE Tarea SET id_tablero = @idTablero, nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color, id_usuario_asignado = @idUsuario WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idTablero", tarea.idTablero);
            command.Parameters.AddWithValue("@nombre", tarea.nombre);
            command.Parameters.AddWithValue("@estado", tarea.estado);
            command.Parameters.AddWithValue("@descripcion", tarea.descripcion);
            command.Parameters.AddWithValue("@color", tarea.color);
            command.Parameters.AddWithValue("@idUsuario", tarea.idUsuarioAsignado);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public Tareas ObtenerDetallesDeTarea(int id){
        Tareas tarea = new Tareas();
        string QueryString = @"SELECT id_tablero, nombre, estado, descripcion, color, id_usuario_asignado FROM Tarea WHERE id = @id";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader = command.ExecuteReader()){
                if(reader.Read()){
                    tarea.id = id;
                    tarea.idTablero = Convert.ToInt32(reader["id_tablero"]);
                    var nombre = reader["nombre"].ToString();
                    if(nombre != null){
                        tarea.nombre = nombre;
                    }
                    tarea.estado = (Tareas.EstadoTarea)Convert.ToInt32(reader["estado"]);
                    var descripcion = reader["descripcion"].ToString();
                    if(descripcion != null){
                        tarea.descripcion = descripcion;
                    }
                    var color = reader["color"].ToString();
                    if(color != null){
                        tarea.color = color;
                    }
                    tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                }
            }
            connection.Close();
        }
        return tarea;
    }
    public List<Tareas> ListarTareasDeUsuario(int idUsuario){
        List<Tareas> listaDeTareas = new List<Tareas>();
        string QueryString = @"SELECT * FROM Tarea WHERE id_usuario_asignado = @idUsuario;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idUsuario", idUsuario);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    Tareas tarea = new Tareas();
                    tarea.id = Convert.ToInt32(reader["id"]);
                    tarea.idTablero = Convert.ToInt32(reader["id_tablero"]);
                    var nombre = reader["nombre"].ToString();
                    if(nombre != null){
                        tarea.nombre = nombre;
                    }
                    tarea.estado = (Tareas.EstadoTarea)Convert.ToInt32(reader["estado"]);
                    var descripcion = reader["descripcion"].ToString();
                    if(descripcion != null){
                        tarea.descripcion = descripcion;
                    }
                    var color = reader["color"].ToString();
                    if(color != null){
                        tarea.color = color;
                    }
                    tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    listaDeTareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return listaDeTareas;
    }
    public List<Tareas> ListarTareasDeTablero(int idTablero){
        List<Tareas> listaDeTareas = new List<Tareas>();
        string QueryString = @"SELECT * FROM Tarea WHERE id_tablero = @idTablero;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idTablero", idTablero);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    Tareas tarea = new Tareas();
                    tarea.id = Convert.ToInt32(reader["id"]);
                    tarea.idTablero = Convert.ToInt32(reader["id_tablero"]);
                    var nombre = reader["nombre"].ToString();
                    if(nombre != null){
                        tarea.nombre = nombre;
                    }
                    tarea.estado = (Tareas.EstadoTarea)Convert.ToInt32(reader["estado"]);
                    var descripcion = reader["descripcion"].ToString();
                    if(descripcion != null){
                        tarea.descripcion = descripcion;
                    }
                    var color = reader["color"].ToString();
                    if(color != null){
                        tarea.color = color;
                    }
                    tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    listaDeTareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return listaDeTareas;
    }
    public void AsignarUsuarioATarea(int idUsuario, int idTarea){
        string QueryString = @"UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE id = @idTarea;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idUsuario", idUsuario);
            command.Parameters.AddWithValue("@idTarea", idTarea);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void EliminarTarea(int idTarea){
        string QueryString = @"DELETE FROM Tarea WHERE id = @idTarea;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idTarea", idTarea);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}