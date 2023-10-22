using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class PiezaT : IPieza
    {
        public Casilla[,] tablero { get; set; }
        public bool seMovio { get; set; }

        public PiezaT()
        {
            tablero = new Casilla[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tablero[i, j] = Casilla.Libre;
                }
            }
            seMovio = false; 
        }

        public bool EsMovimientoSeguro(int fila, int columna)
        {
            if (seMovio)
            {
                return false;
            }

            if (fila < 0 || columna < 0 || fila >= 8 || columna >= 8 || tablero[fila, columna] == Casilla.Ocupada)
            {
                return false;
            }

            int[] deltaFila = { -2, 0, 2, 0 };
            int[] deltaColumna = { 0, -2, 0, 2 };

            for (int i = 0; i < deltaFila.Length; i++)
            {
                int nuevaFila = fila + deltaFila[i];
                int nuevaColumna = columna + deltaColumna[i];

                if (nuevaFila >= 0 && nuevaFila < 8 && nuevaColumna >= 0 && nuevaColumna < 8)
                {
                    if (tablero[nuevaFila, nuevaColumna] == Casilla.Ocupada)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool Backtracking(int pPiezas)
        {
            if (pPiezas == 8)
            {
                return true;
            }

            for (int columna = 0; columna < 8; columna++)
            {
                if (EsMovimientoSeguro(pPiezas, columna))
                {
                    tablero[pPiezas, columna] = Casilla.Ocupada;
                    seMovio = true;

                    if (Backtracking(pPiezas + 1))
                    {
                        return true;
                    }

                    tablero[pPiezas, columna] = Casilla.Libre;
                    seMovio = false;
                }
            }

            throw new SinSolucionException();
        }

        public void MostrarPosicion()
        {
            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    if (tablero[fila, columna] == Casilla.Ocupada)
                    {
                        Console.WriteLine("PiezaT en fila {0}, columna {1}", fila, columna);
                    }
                }
            }
        }
    }
}

