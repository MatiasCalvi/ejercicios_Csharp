using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Reina : IPieza
    {
        public Casilla[,] tablero { get; set; }

        public Reina()
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

            for (int i = 0; i < 8; i++)
            {
                if (tablero[i, columna] == Casilla.Ocupada || tablero[fila, i] == Casilla.Ocupada)
                {
                    return false;
                }

                for (int j = 0; j < 8; j++)
                {
                    if (i + j == fila + columna || i - j == fila - columna)
                    {
                        if (tablero[i, j] == Casilla.Ocupada)
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
                return true;
            }

            for (int columna = 0; columna < 8; columna++)
            {
                if (EsMovimientoSeguro(pPiezas, columna))
                {
                    tablero[pPiezas, columna] = Casilla.Ocupada;

                    for (int i = 0; i < 8; i++)
                    {
                        if (tablero[i, columna] == Casilla.Libre)
                        {
                            tablero[i, columna] = Casilla.Marcado;
                        }

                        if (tablero[pPiezas, i] == Casilla.Libre)
                        {
                            tablero[pPiezas, i] = Casilla.Marcado;
                        }

                        for (int j = 0; j < 8; j++)
                        {
                            if (i + j == pPiezas + columna || i - j == pPiezas - columna)
                            {
                                if (tablero[i, j] == Casilla.Libre)
                                {
                                    tablero[i, j] = Casilla.Marcado;
                                }
                            }
                        }
                    }

                    if (Backtracking(pPiezas + 1))
                    {
                        return true;
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        if (tablero[i, columna] == Casilla.Marcado)
                        {
                            tablero[i, columna] = Casilla.Libre;
                        }

                        if (tablero[pPiezas, i] == Casilla.Marcado)
                        {
                            tablero[pPiezas, i] = Casilla.Libre;
                        }

                        for (int j = 0; j < 8; j++)
                        {
                            if (i + j == pPiezas + columna || i - j == pPiezas - columna)
                            {
                                if (tablero[i, j] == Casilla.Marcado)
                                {
                                    tablero[i, j] = Casilla.Libre;
                                }
                            }
                        }
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
                        Console.WriteLine("Reina en fila {0}, columna {1}", fila, columna);
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

