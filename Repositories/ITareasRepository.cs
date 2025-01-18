public interface ITareasRepository{
    void CrearTarea(int idTablero, Tareas tarea);
    void ModificarTarea(int id, Tareas tarea);
    Tareas ObtenerDetallesDeTarea(int id);
    List<Tareas> ListarTareasDeUsuario(int idUsuario);
    List<Tareas> ListarTareasDeTablero(int idTablero);
    void AsignarUsuarioATarea(int idUsuario, int idTarea);
    void EliminarTarea(int idTarea);
}