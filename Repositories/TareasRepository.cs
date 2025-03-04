using Microsoft.Data.Sqlite;
using tl2_proyecto_2024_Olme2.Models;
public class TareasRepository : ITareasRepository{

    private readonly string ConnectionString;

    //Con inyección de dependencias.
    public TareasRepository(string cadenaDeConexion){
        ConnectionString = cadenaDeConexion;
    }

    //Método para crear una nueva tarea.
    public void CrearTarea(int idTablero, Tareas tarea){ 
        
        string QueryString = @"INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color) VALUES(@idTablero, @nombre, @estado, @descripcion, @color);";
        
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
        
            connection.Open();
        
            SqliteCommand command = new SqliteCommand(QueryString, connection);
        
            command.Parameters.AddWithValue("@idTablero", idTablero);
            command.Parameters.AddWithValue("@nombre", tarea.nombre);
            command.Parameters.AddWithValue("@estado", tarea.estado);
            //Se verifica si la descripcion es nula o no en la BD, ya que puede serlo.
            if(tarea.descripcion!=null){ 
                command.Parameters.AddWithValue("@descripcion", tarea.descripcion);
            }else{
                command.Parameters.AddWithValue("@descripcion", DBNull.Value);
            }
            if(tarea.color!=null){ //Se verifica si el color es nulo o no para mandarlo a la BD, ya que puede serlo.
                command.Parameters.AddWithValue("@color", tarea.color);
            }else{
                command.Parameters.AddWithValue("@color", DBNull.Value);
            }

            command.ExecuteNonQuery();
            
            connection.Close();
        
        }

    }

    //Método para modificar una determinada tarea.
    public void ModificarTarea(int id, Tareas tarea){ 

        string QueryString = @"UPDATE Tarea SET id_tablero = @idTablero, nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color WHERE id = @id;";
        
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
        
            connection.Open();
        
            SqliteCommand command = new SqliteCommand(QueryString, connection);
        
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idTablero", tarea.idTablero);
            command.Parameters.AddWithValue("@nombre", tarea.nombre);
            command.Parameters.AddWithValue("@estado", tarea.estado);
            //Se verifica si la descripcion es nula o no para mandarlo a la BD, ya que puede serlo.
            if(tarea.descripcion!=null){ 
                command.Parameters.AddWithValue("@descripcion", tarea.descripcion);
            }else{
                command.Parameters.AddWithValue("@descripcion", DBNull.Value);
            }
            if(tarea.color!=null){ //Se verifica si el color es nulo o no para mandarlo a la BD, ya que puede serlo.
                command.Parameters.AddWithValue("@color", tarea.color);
            }else{
                command.Parameters.AddWithValue("@color", DBNull.Value);
            }

            command.ExecuteNonQuery();
            
            connection.Close();
        }

    }

    //Método para obtener los detalles de una determinada tarea.
    public Tareas ObtenerDetallesDeTarea(int id){ 
        
        Tareas? tarea = null;
        
        string QueryString = @"SELECT id_tablero, nombre, estado, descripcion, color, id_usuario_asignado FROM Tarea WHERE id = @id";
        
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
        
            connection.Open();
        
            SqliteCommand command = new SqliteCommand(QueryString, connection);
        
            command.Parameters.AddWithValue("@id", id);
        
            using(SqliteDataReader reader = command.ExecuteReader()){
        
                if(reader.Read()){
        
                    tarea = new Tareas();
                    tarea.id = id;
                    tarea.idTablero = Convert.ToInt32(reader["id_tablero"]);
                    var nombre = reader["nombre"].ToString();
                    //El nombre no puede ser nulo, pero igual lo verifico para que no salte error.
                    if(nombre != null){ 
                        tarea.nombre = nombre;
                    }
                    tarea.estado = (Tareas.EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.descripcion = reader["descripcion"].ToString();
                    tarea.color = reader["color"].ToString();
                    //Se verifica si el id del usuario asignado es nulo o no en la BD, ya que puede serlo.
                    if(reader["id_usuario_asignado"] != DBNull.Value){ 
                        tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    }else{
                        tarea.idUsuarioAsignado = null;
                    }

                }

            }

            connection.Close();
        
        }
        
        if(tarea==null){
            throw new Exception("Tarea Inexistente.");
        }

        return tarea;

    }

    //Método para listar todas las tareas.
    public List<Tareas> ListarTareas(){ 

        List<Tareas> tareas = new List<Tareas>();

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
                    //El nombre no puede ser nulo, pero igual lo verifico para que no salte error.
                    if(nombre != null){ 
                        tarea.nombre = nombre;
                    }
                    tarea.estado = (Tareas.EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.descripcion = reader["descripcion"].ToString();
                    tarea.color = reader["color"].ToString();
                    //Se verifica si el id del usuario asignado es nulo o no en la BD, ya que puede serlo.
                    if(reader["id_usuario_asignado"] != DBNull.Value){ 
                        tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    }else{
                        tarea.idUsuarioAsignado = null;
                    }

                    tareas.Add(tarea);

                }

            }

            connection.Close();
        
        }

        return tareas;

    }

    //Método para listar solo las tareas asignadas a un determinado usuario.
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
                    //El nombre no puede ser nulo, pero igual lo verifico para que no salte error.
                    if(nombre != null){ 
                        tarea.nombre = nombre;
                    }
                    tarea.estado = (Tareas.EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.descripcion = reader["descripcion"].ToString();
                    tarea.color = reader["color"].ToString();
                    //En este caso se que el id de usuario asignado no es nulo, ya que estoy buscando tareas de un determinado usuario.
                    tarea.idUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]); 

                    listaDeTareas.Add(tarea);
    
                }
    
            }
    
            connection.Close();
    
        }
    
        return listaDeTareas;
    
    }

    //Método para solo las tareas pertenecientes a un determinado tablero
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
                    //El nombre no puede ser nulo, pero igual lo verifico para que no salte error.
                    if(nombre != null){ 
                        tarea.nombre = nombre;
                    }
                    tarea.estado = (Tareas.EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.descripcion = reader["descripcion"].ToString();
                    tarea.color = reader["color"].ToString();
                    //Se verifica si el id del usuario asignado es nulo o no en la BD, ya que puede serlo.
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

    //Método para asignar un usuario a una tarea. Puede no recibir ningun usuario y se asignara nulo.
    public void AsignarUsuarioATarea(int? idUsuario, int idTarea){ 
    
        string QueryString = @"UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE id = @idTarea;";
    
        using(SqliteConnection connection = new SqliteConnection(ConnectionString)){
    
            connection.Open();
    
            SqliteCommand command = new SqliteCommand(QueryString, connection);
    
            //Se verifica si el id del usuario asignado es nulo o no para mandarlo a la BD, ya que puede serlo. Si el id es -1 es nulo.
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

    //Método para eliminar una determinada tarea.
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