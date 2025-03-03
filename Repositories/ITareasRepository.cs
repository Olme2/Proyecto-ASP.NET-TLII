using tl2_proyecto_2024_Olme2.Models;
public interface ITareasRepository{
    void CrearTarea(int idTablero, Tareas tarea); //Método para crear una nueva tarea.
    void ModificarTarea(int id, Tareas tarea); //Método para modificar una determinada tarea.
    Tareas ObtenerDetallesDeTarea(int id); //Método para obtener los detalles de una determinada tarea.
    List<Tareas> ListarTareas(); //Método para listar todas las tareas.
    List<Tareas> ListarTareasDeUsuario(int idUsuario); //Método para listar solo las tareas asignadas a un determinado usuario.
    List<Tareas> ListarTareasDeTablero(int idTablero); //Método para solo las tareas pertenecientes a un determinado tablero.
    void AsignarUsuarioATarea(int? idUsuario, int idTarea); //Método para asignar un usuario a una tarea. Puede no recibir ningun usuario y se asignara nulo.
    void EliminarTarea(int idTarea); //Método para eliminar una determinada tarea.
}