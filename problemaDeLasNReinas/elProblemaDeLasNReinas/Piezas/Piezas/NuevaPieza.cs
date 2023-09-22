using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class NuevaPieza
    {
        public bool EsMovimientoSeguro(Casilla[,] tablero, int fila, int columna)
        {
            

            for (int i = 0; i < 8; i++)
            {
                if (tablero[i, columna] == Casilla.Ocupada)
                {
                    return false;
                }
            }

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nuevaFila = fila + i;
                    int nuevaColumna = columna + j;

                    if (nuevaFila >= 0 && nuevaFila < 8 && nuevaColumna >= 0 && nuevaColumna < 8)
                    {
                        if (tablero[nuevaFila, nuevaColumna] == Casilla.Ocupada)
                        {
                            return false;
                        }
                    }
                }
            }

         
            




            if (fila < 0 || columna < 0 || fila >= 8 || columna >= 8)
            {
                return false; 
            }

            if (tablero[fila, columna] == Casilla.Ocupada)
            {
                return false; 
            }

           
            int[] movimientosX = { -2, -1, 1, 2, -2, -1, 1, 2 };
            int[] movimientosY = { -1, -2, -2, -1, 1, 2, 2, -1 };

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
    }
}
