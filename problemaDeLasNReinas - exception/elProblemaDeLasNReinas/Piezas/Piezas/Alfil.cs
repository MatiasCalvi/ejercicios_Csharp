using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Alfil : IPieza
    {
        public Casilla[,] Tablero { get; set; }

        public Alfil()
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

            for (int i = 0; i < 8; i++)
            {
                int deltaFila = Math.Abs(i - fila);
                int deltaColumna = Math.Abs(i - columna);

                if (Tablero[i, i] == Casilla.Ocupada && deltaFila == deltaColumna)
                {
                    return false;
                }

                if (Tablero[i, 7 - i] == Casilla.Ocupada && deltaFila == deltaColumna)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Backtracking(int filaActual)
        {
            if (filaActual == 8)
            {
                for (int fila = 0; fila < 8; fila++)
                {
                    for (int columna = 0; columna < 8; columna++)
                    {
                        if (Tablero[fila, columna] == Casilla.Ocupada)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                int deltaFila = Math.Abs(i - fila);
                                int deltaColumna = Math.Abs(i - columna);

                                if (Tablero[i, i] == Casilla.Libre && deltaFila == deltaColumna)
                                {
                                    Tablero[i, i] = Casilla.Marcado;
                                }

                                if (Tablero[i, 7 - i] == Casilla.Libre && deltaFila == deltaColumna)
                                {
                                    Tablero[i, 7 - i] = Casilla.Marcado;
                                }
                            }
                        }
                    }
                }

                return true;
            }

            for (int columna = 0; columna < 8; columna++)
            {
                if (EsMovimientoSeguro(filaActual, columna))
                {
                    Tablero[filaActual, columna] = Casilla.Ocupada;

                    if (Backtracking(filaActual + 1))
                    {
                        return true;
                    }

                    Tablero[filaActual, columna] = Casilla.Libre;
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
                        Console.WriteLine("Alfil en fila {0}, columna {1}", fila, columna);
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

