﻿using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Caballo : IPieza
    {
        public Casilla[,] Tablero { get; set; }
        private int[] movimientosX = { 2, 1, -1, -2, -2, -1, 1, 2 };
        private int[] movimientosY = { 1, 2, 2, 1, -1, -2, -2, -1 };

        public Caballo()
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


            for (int i = 0; i < 8; i++)
            {
                if (Tablero[i, columna] == Casilla.Ocupada || Tablero[fila, i] == Casilla.Ocupada)
                {
                    return false;
                }

                for (int j = 0; j < 8; j++)
                {

                    for (int indice = 0; indice < movimientosX.Length; indice++)
                    {
                        if (fila + movimientosX[indice] > fila && fila + movimientosX[indice] < 8)
                        {
                            if (columna + movimientosY[indice] > columna && columna + movimientosY[indice] < 8)
                            {
                                if (Tablero[movimientosX[indice], movimientosY[indice]] == Casilla.Ocupada)
                                {
                                    return false;
                                }
                            }
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

            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    if (EsMovimientoSeguro(fila, columna))
                    {
                        Tablero[fila, columna] = Casilla.Ocupada;

                        for (int indice = 0; indice < movimientosX.Length; indice++)
                        {
                            int nuevaFila = fila + movimientosX[indice];
                            int nuevaColumna = columna + movimientosY[indice];

                            if (nuevaFila >= 0 && nuevaFila < 8 && nuevaColumna >= 0 && nuevaColumna < 8)
                            {
                                if (Tablero[nuevaFila, nuevaColumna] == Casilla.Libre)
                                {
                                    Tablero[nuevaFila, nuevaColumna] = Casilla.Marcado;
                                }
                            }
                        }

                        if (Backtracking(pPiezas + 1))
                        {
                            return true;
                        }

                        for (int indice = 0; indice < movimientosX.Length; indice++)
                        {
                            int nuevaFila = fila + movimientosX[indice];
                            int nuevaColumna = columna + movimientosY[indice];

                            if (nuevaFila >= 0 && nuevaFila < 8 && nuevaColumna >= 0 && nuevaColumna < 8)
                            {
                                if (Tablero[nuevaFila, nuevaColumna] == Casilla.Marcado)
                                {
                                    Tablero[nuevaFila, nuevaColumna] = Casilla.Libre;
                                }
                            }
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
                        Console.WriteLine("Caballo en fila {0}, columna {1}", fila, columna);
                    }
                    //else if (tablero[fila, columna] == Casilla.Marcado)
                    //{
                    //    Console.WriteLine("Casilla marcada en fila {0}, columna {1}", fila, columna);
                    //}
                }
            }
        }
    }
}
