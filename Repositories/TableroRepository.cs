using Microsoft.Data.Sqlite;
using tl2_proyecto_2024_Olme2.Models;
public class TableroRepository : ITableroRepository{
    private readonly string ConnectionString;
    public TableroRepository(string connectionString){
        ConnectionString = connectionString; //Con inyección de dependencias.
    }
    public void CrearTablero(Tablero tablero){ //Método para crear tableros en la base de datos.
        string QueryString = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) VALUES(@idPropietario, @nombre, @descripcion);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idPropietario", tablero.idUsuarioPropietario);
            command.Parameters.AddWithValue("@nombre", tablero.nombre);
            if(tablero.descripcion != null){ //Como la descripción puede ser nula, se la analiza antes de asignar.
                command.Parameters.AddWithValue("@descripcion", tablero.descripcion);
            }else{
                command.Parameters.AddWithValue("@descripcion", DBNull.Value);
            }
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void ModificarTablero(int id, Tablero tablero){ //Método para modificar tableros en la base de datos.
        string QueryString = @"UPDATE Tablero SET id_usuario_propietario = @idUsuario, nombre = @nombre, descripcion = @descripcion WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idUsuario", tablero.idUsuarioPropietario);
            command.Parameters.AddWithValue("@nombre", tablero.nombre);
            if(tablero.descripcion != null){ //Como la descripción puede ser nula, se la analiza antes de asignar.
                command.Parameters.AddWithValue("@descripcion", tablero.descripcion);
            }else{
                command.Parameters.AddWithValue("@descripcion", DBNull.Value);
            }
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public Tablero ObtenerDetallesDeTablero(int id){ //Método para obtener un tablero específico por id.
        Tablero tablero = new Tablero();
        string QueryString = @"SELECT id_usuario_propietario, nombre, descripcion FROM Tablero WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader = command.ExecuteReader()){
                if(reader.Read()){
                    tablero.id = id;
                    tablero.idUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    var nombre = reader["nombre"].ToString();
                    if(nombre != null){ //El nombre no puede ser nulo, pero me saltaba que podia asignarse valor nulo igual, asi que hice un analisis.
                        tablero.nombre = nombre;
                    }
                    tablero.descripcion = reader["descripcion"].ToString();
                }
            }
            connection.Close();
        }
        return tablero;
    }
    public List<Tablero> ListarTableros(){ //Método para obtener todos los tableros, sin restricciones.
        List<Tablero> listaDeTableros = new List<Tablero>();
        string QueryString = @"SELECT * FROM Tablero;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    Tablero tablero = new Tablero();
                    tablero.id = Convert.ToInt32(reader["id"]);
                    tablero.idUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    var nombre = reader["nombre"].ToString();
                    var descripcion = reader["descripcion"].ToString();
                    if(nombre != null){ //El nombre no puede ser nulo, pero me saltaba que podia asignarse valor nulo igual, asi que hice un analisis.
                        tablero.nombre = nombre;
                    }
                    tablero.descripcion = descripcion;
                    listaDeTableros.Add(tablero);
                }
            }
            connection.Close();
        }
        return listaDeTableros;
    }
    public List<Tablero> ListarTablerosDeUsuario(int idUsuario){ //Método para obtener los tableros creados por un usuario en específico.
        List<Tablero> listaDeTableros = new List<Tablero>();
        string QueryString = @"SELECT * FROM Tablero WHERE id_usuario_propietario = @idUsuario;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idUsuario", idUsuario);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    Tablero tablero = new Tablero();
                    tablero.id = Convert.ToInt32(reader["id"]);
                    tablero.idUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    var nombre = reader["nombre"].ToString();
                    var descripcion = reader["descripcion"].ToString();
                    if(nombre != null){
                        tablero.nombre = nombre;
                    }
                    tablero.descripcion = descripcion;
                    listaDeTableros.Add(tablero);
                }
            }
            connection.Close();
        }
        return listaDeTableros;
    }
    public List<Tablero> ListarTablerosConTareasAsignadas(int idUsuario){ //Método para obtener los tableros que tengan al menos una tarea asignada para un usuario en específico y que el mismo no sea dueño del tablero.
        List<Tablero> tableros = new List<Tablero>();
        string QueryString = @"SELECT DISTINCT t.* FROM Tablero t INNER JOIN Tarea ta ON t.id = ta.id_tablero WHERE ta.id_usuario_asignado = @idUsuario AND t.id_usuario_propietario <> @idUsuario;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idUsuario", idUsuario);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    Tablero tablero = new Tablero();
                    tablero.id = Convert.ToInt32(reader["id"]);
                    tablero.idUsuarioPropietario = Convert.ToInt32(idUsuario);
                    var nombre = reader["nombre"].ToString();
                    var descripcion = reader["descripcion"].ToString();
                    if(nombre != null){ //El nombre no puede ser nulo, pero me saltaba que podia asignarse valor nulo igual, asi que hice un analisis.
                        tablero.nombre = nombre;
                    }
                    tablero.descripcion = descripcion;
                    tableros.Add(tablero);
                }
            }
        }
        return tableros;
    }
    public void EliminarTableroPorId(int id){ //Método para eliminar tablero por id.
        string QueryString = @"DELETE FROM Tablero WHERE id = @id AND id NOT IN (SELECT id_tablero FROM Tarea WHERE id_tablero = @id);"; //Se borra solo si no tiene tareas asignadas.
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}