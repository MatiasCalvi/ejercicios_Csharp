﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class Peon
    {
        public Casilla[,] tablero { get; set; }

        public Peon()
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

            return true;
        }

        public bool Backtracking(int filaActual)
        {
            if (filaActual == 8)
            {
                return true;
            }

            for (int columna = 0; columna < 8; columna++)
            {
                if (EsMovimientoSeguro(filaActual, columna))
                {
                    tablero[filaActual, columna] = Casilla.Ocupada;

                    if (Backtracking(filaActual + 1))
                    {
                        return true;
                    }

                    tablero[filaActual, columna] = Casilla.Libre;
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
                        Console.WriteLine("Peon en fila {0}, columna {1}", fila, columna);
                    }
                }
            }
        }
    }
}
