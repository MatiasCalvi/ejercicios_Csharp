using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Torre : IPieza
    {
        public Casilla[,] Tablero { get; set; }

        public Torre()
        {
            Tablero = new Casilla[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Tablero[i, j] = Casilla.Libre;
                }
            }
        }

        public bool EsMovimientoSeguro(int fila, int columna)
        {

            for (int i = 0; i < fila; i++)
            {
                if (Tablero[i, columna] == Casilla.Ocupada)
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
                        if (Tablero[fila, columna] == Casilla.Ocupada)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                if (Tablero[i, columna] == Casilla.Libre)
                                {
                                    Tablero[i, columna] = Casilla.Marcado;
                                }

                                if (Tablero[fila, i] == Casilla.Libre)
                                {
                                    Tablero[fila, i] = Casilla.Marcado;
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
                    Tablero[pPiezas, columna] = Casilla.Ocupada;

                    if (Backtracking(pPiezas + 1))
                    {
                        return true;
                    }

                    Tablero[pPiezas, columna] = Casilla.Libre;
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
                    if (Tablero[fila, columna] == Casilla.Ocupada)
                    {
                        Console.WriteLine("Torre en fila {0}, columna {1}", fila, columna);
                    }
                    else if (Tablero[fila, columna] == Casilla.Marcado)
                    {
                        //Console.WriteLine("Casilla marcada en fila {0}, columna {1}", fila, columna);
                    }
                }
            }
        }
    }
}

