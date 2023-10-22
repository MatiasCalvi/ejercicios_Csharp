using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Rey : IPieza
    {
        public Casilla[,] Tablero { get; set; }

        public Rey()
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

            if (fila < 0 || columna < 0 || fila >= 8 || columna >= 8 || Tablero[fila, columna] == Casilla.Ocupada)
            {
                return false;
            }

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nuevaFila = fila + i;
                    int nuevaColumna = columna + j;

                    if (nuevaFila >= 0 && nuevaFila < 8 && nuevaColumna >= 0 && nuevaColumna < 8)
                    {
                        if (Tablero[nuevaFila, nuevaColumna] == Casilla.Ocupada)
                        {
                            return false;
                        }
                    }
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
                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    int nuevaFila = fila + i;
                                    int nuevaColumna = columna + j;

                                    if (nuevaFila >= 0 && nuevaFila < 8 && nuevaColumna >= 0 && nuevaColumna < 8)
                                    {
                                        if (Tablero[nuevaFila, nuevaColumna] == Casilla.Libre)
                                        {
                                            Tablero[nuevaFila, nuevaColumna] = Casilla.Marcado;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }

            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    if (EsMovimientoSeguro(fila, columna))
                    {
                        Tablero[fila, columna] = Casilla.Ocupada;

                        if (Backtracking(pPiezas + 1))
                        {
                            return true;
                        }

                        Tablero[fila, columna] = Casilla.Libre;
                    }
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
                        Console.WriteLine("Rey en fila {0}, columna {1}", fila, columna);
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


