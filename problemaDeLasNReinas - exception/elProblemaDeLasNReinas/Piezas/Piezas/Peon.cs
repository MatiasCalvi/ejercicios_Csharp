using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Peon : IPieza
    {
        public Casilla[,] Tablero { get; set; }

        public Peon()
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
            if (fila < 0 || columna < 0 || fila >= 8 || columna >= 8)
            {
                return false;
            }

            if (Tablero[fila, columna] == Casilla.Ocupada)
            {
                return false;
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
                            int nuevaFila = fila + 1;
                            if (nuevaFila < 8 && Tablero[nuevaFila, columna] == Casilla.Libre)
                            {
                                Tablero[nuevaFila, columna] = Casilla.Marcado;
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
                        Console.WriteLine("Peon en fila {0}, columna {1}", fila, columna);
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

