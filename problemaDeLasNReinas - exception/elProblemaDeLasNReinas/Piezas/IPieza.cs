namespace Piezas
{
    public interface IPieza
    {
        enum Casilla { Libre = 0, Ocupada = 1, Marcado = 2 } 
        Casilla[,] tablero { get; set; }
        bool EsMovimientoSeguro(int fila, int columna);
        bool Backtracking(int pPiezas);
    }
}
