using tl2_proyecto_2024_Olme2.Models;
public interface ITableroRepository{
    void CrearTablero(Tablero tablero);//Método para crear tableros en la base de datos.
    void ModificarTablero(int id, Tablero tablero); //Método para modificar tableros en la base de datos.
    Tablero ObtenerDetallesDeTablero(int id); //Método para obtener un tablero específico por id.
    List<Tablero> ListarTableros(); //Método para obtener todos los tableros, sin restricciones.
    List<Tablero> ListarTablerosDeUsuario(int idUsuario); //Método para obtener los tableros creados por un usuario en específico.
    List<Tablero> ListarTablerosConTareasAsignadas(int idUsuario); //Método para obtener los tableros que tengan al menos una tarea asignada para un usuario en específico y que el mismo no sea dueño del tablero.
    void EliminarTableroPorId(int id); //Método para eliminar tablero por id.
}