using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Caballo
    {
        public Casilla[,] tablero { get; set; }
        private int[] movimientosX = { 2, 1, -1, -2, -2, -1, 1, 2 };
        private int[] movimientosY = { 1, 2, 2, 1, -1, -2, -2, -1 };

        public Caballo()
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
            if (fila < 0 || columna < 0 || fila >= 8 || columna >= 8)
            {
                return false;
            }

            if (tablero[fila, columna] == Casilla.Ocupada)
            {
                return false;
            }

            for (int i = 0; i < movimientosX.Length; i++)
            {
                int nuevaFila = fila + movimientosX[i];
                int nuevaColumna = columna + movimientosY[i];

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

        public bool Backtracking(int caballosColocados)
        {
            if (caballosColocados == 8)
            {
                return true;
            }

            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                {
                    if (EsMovimientoSeguro(fila, columna))
                    {
                        tablero[fila, columna] = Casilla.Ocupada;

                        if (Backtracking(caballosColocados + 1))
                        {
                            return true;
                        }

                        tablero[fila, columna] = Casilla.Libre;
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
                    if (tablero[fila, columna] == Casilla.Ocupada)
                    {
                        Console.WriteLine("Caballo en fila {0}, columna {1}", fila, columna);
                    }
                }
            }
        }
    }
}
