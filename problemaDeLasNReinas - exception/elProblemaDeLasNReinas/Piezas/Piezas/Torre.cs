using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Torre : IPieza
    {
        public Casilla[,] tablero { get; set; }

        public Torre()
        {
            tablero = new Casilla[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tablero[i, j] = Casilla.Libre;
                }
            }
        }

        public bool EsMovimientoSeguro(int fila, int columna)
        {

            for (int i = 0; i < fila; i++)
            {
                if (tablero[i, columna] == Casilla.Ocupada)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Backtracking(int pPiezas)
        {
            if (pPiezas == 8)
            {
                for (int fila = 0; fila < 8; fila++)
                {
                    for (int columna = 0; columna < 8; columna++)
                    {
                        if (tablero[fila, columna] == Casilla.Ocupada)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                if (tablero[i, columna] == Casilla.Libre)
                                {
                                    tablero[i, columna] = Casilla.Marcado;
                                }

                                if (tablero[fila, i] == Casilla.Libre)
                                {
                                    tablero[fila, i] = Casilla.Marcado;
                                }
                            }
                        }
                    }
                }

                return true;
            }

            for (int columna = 0; columna < 8; columna++)
            {
                if (EsMovimientoSeguro(pPiezas, columna))
                {
                    tablero[pPiezas, columna] = Casilla.Ocupada;

                    if (Backtracking(pPiezas + 1))
                    {
                        return true;
                    }

                    tablero[pPiezas, columna] = Casilla.Libre;
                }
            }

            return false;
        }

        public void MostrarPosicion()
        {
            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    if (tablero[fila, columna] == Casilla.Ocupada)
                    {
                        Console.WriteLine("Torre en fila {0}, columna {1}", fila, columna);
                    }
                    else if (tablero[fila, columna] == Casilla.Marcado)
                    {
                        //Console.WriteLine("Casilla marcada en fila {0}, columna {1}", fila, columna);
                    }
                }
            }
        }
    }
}

