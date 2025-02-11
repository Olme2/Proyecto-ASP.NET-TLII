using Microsoft.Data.Sqlite;
using tl2_proyecto_2024_Olme2.Models;
public class TableroRepository : ITableroRepository{
    private readonly string ConnectionString;
    public TableroRepository(string connectionString){
        ConnectionString = connectionString;
    }
    public void CrearTablero(Tablero tablero){
        string QueryString = @"INSERT INTO Tablero VALUES(@idPropietario, @nombre, @descripcion);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@idPropietario", tablero.idUsuarioPropietario);
            command.Parameters.AddWithValue("@nombre", tablero.nombre);
            command.Parameters.AddWithValue("@descripcion", tablero.descripcion);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void ModificarTablero(int id, Tablero tablero){
        string QueryString = @"UPDATE Tablero SET id_usuario_propietario = @idUsuario, nombre = @nombre, descripcion = @descripcion WHERE id = @id;";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", tablero.id);
            command.Parameters.AddWithValue("@idUsuario", tablero.idUsuarioPropietario);
            command.Parameters.AddWithValue("@nombre", tablero.nombre);
            command.Parameters.AddWithValue("@descripcion", tablero.descripcion);
            connection.Close();
        }
    }
    public Tablero ObtenerDetallesDeTablero(int id){
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
                    var descripcion = reader["descripcion"].ToString();
                    if(nombre != null){
                        tablero.nombre = nombre;
                    }
                    if(descripcion != null){
                        tablero.descripcion = descripcion;
                    }
                }
            }
            connection.Close();
        }
        return tablero;
    }
    public List<Tablero> ListarTableros(){
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
                    if(nombre != null){
                        tablero.nombre = nombre;
                    }
                    if(descripcion != null){
                        tablero.descripcion = descripcion;
                    }
                    listaDeTableros.Add(tablero);
                }
            }
            connection.Close();
        }
        return listaDeTableros;
    }
    public List<Tablero> ListarTablerosDeUsuario(int idUsuario){
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
                    if(descripcion != null){
                        tablero.descripcion = descripcion;
                    }
                    listaDeTableros.Add(tablero);
                }
            }
            connection.Close();
        }
        return listaDeTableros;
    }
    public void EliminarTableroPorId(int id){
        string QueryString = @"DELETE FROM Tablero WHERE id = @id AND id NOT IN (SELECT id_tablero FROM Tarea WHERE id_tablero = @id);";
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(QueryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}