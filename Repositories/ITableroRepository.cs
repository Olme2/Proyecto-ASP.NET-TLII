using tl2_proyecto_2024_Olme2.Models;
public interface ITableroRepository{
    void CrearTablero(Tablero tablero);
    void ModificarTablero(int id, Tablero tablero);
    Tablero ObtenerDetallesDeTablero(int id);
    List<Tablero> ListarTableros();
    List<Tablero> ListarTablerosDeUsuario(int idUsuario);
    void EliminarTableroPorId(int id);
}