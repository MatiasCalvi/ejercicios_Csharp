using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Reina : IPieza
    {
        public Casilla[,] Tablero { get; set; }

        public Reina()
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
                if (Tablero[i, columna] == Casilla.Ocupada || Tablero[fila, i] == Casilla.Ocupada)
                {
                    return false;
                }

                for (int j = 0; j < 8; j++)
                {
                    if (i + j == fila + columna || i - j == fila - columna)
                    {   
                        if (Tablero[i, j] == Casilla.Ocupada)
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
                    Tablero[pPiezas, columna] = Casilla.Ocupada;

                    for (int i = 0; i < 8; i++)
                    {
                        if (Tablero[i, columna] == Casilla.Libre)
                        {
                            Tablero[i, columna] = Casilla.Marcado;
                        }

                        if (Tablero[pPiezas, i] == Casilla.Libre)
                        {
                            Tablero[pPiezas, i] = Casilla.Marcado;
                        }

                        for (int j = 0; j < 8; j++)
                        {
                            if (i + j == pPiezas + columna || i - j == pPiezas - columna)
                            {
                                if (Tablero[i, j] == Casilla.Libre)
                                {
                                    Tablero[i, j] = Casilla.Marcado;
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
                        if (Tablero[i, columna] == Casilla.Marcado)
                        {
                            Tablero[i, columna] = Casilla.Libre;
                        }

                        if (Tablero[pPiezas, i] == Casilla.Marcado)
                        {
                            Tablero[pPiezas, i] = Casilla.Libre;
                        }

                        for (int j = 0; j < 8; j++)
                        {
                            if (i + j == pPiezas + columna || i - j == pPiezas - columna)
                            {
                                if (Tablero[i, j] == Casilla.Marcado)
                                {
                                    Tablero[i, j] = Casilla.Libre;
                                }   
                            }
                        }   
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
                        Console.WriteLine("Reina en fila {0}, columna {1}", fila, columna);
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

