using Microsoft.Data.Sqlite;
using tl2_proyecto_2024_Olme2.Models;
public class TareasRepository : ITareasRepository{
    private readonly string ConnectionString;
    public TareasRepository(string cadenaDeConexion){
        ConnectionString = cadenaDeConexion;
    }
    public void CrearTarea(int idTablero, Tareas tarea){
        string QueryString = @"INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color) VALUES(@idTablero, @nombre, @estado, @descripcion, @color);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idTablero", idTablero);
            command.Parameters.AddWithValue("@nombre", tarea.nombre);
            command.Parameters.AddWithValue("@estado", tarea.estado);
            if(tarea.descripcion!=null){
                command.Parameters.AddWithValue("@descripcion", tarea.descripcion);
            }else{
                command.Parameters.AddWithValue("@descripcion", DBNull.Value);
            }
            command.Parameters.AddWithValue("@color", tarea.color);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void ModificarTarea(int id, Tareas tarea){
        string QueryString = @"UPDATE Tarea SET id_tablero = @idTablero, nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idTablero", tarea.idTablero);
            command.Parameters.AddWithValue("@nombre", tarea.nombre);
            command.Parameters.AddWithValue("@estado", tarea.estado);
            if(tarea.descripcion!=null){
                command.Parameters.AddWithValue("@descripcion", tarea.descripcion);
            }else{
                command.Parameters.AddWithValue("@descripcion", DBNull.Value);
            }
            command.Parameters.AddWithValue("@color", tarea.color);
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
                    if(reader["id_usuario_asignado"] != DBNull.Value){
                        tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    }else{
                        tarea.idUsuarioAsignado = null;
                    }
                }
            }
            connection.Close();
        }
        return tarea;
    }
    public List<Tareas> ListarTareas(){
        var tareas = new List<Tareas>();
        string QueryString = @"SELECT * FROM tarea;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
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
                    tarea.descripcion = reader["descripcion"].ToString();
                    tarea.color = reader["color"].ToString();
                    tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    tareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return tareas;
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
                    if(reader["id_usuario_asignado"] != DBNull.Value){
                        tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    }else{
                        tarea.idUsuarioAsignado = null;
                    }
                    listaDeTareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return listaDeTareas;
    }
    public void AsignarUsuarioATarea(int? idUsuario, int idTarea){
        string QueryString = @"UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE id = @idTarea;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            if(idUsuario!=-1){
                command.Parameters.AddWithValue("@idUsuario", idUsuario);
            }else{
                command.Parameters.AddWithValue("@idUsuario", DBNull.Value);
            }
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