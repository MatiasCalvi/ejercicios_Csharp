using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piezas
{
    public interface IPieza
    {
        enum Casilla { Libre = 0, Ocupada = 1 }
        public interface IPiezas
        {
            Casilla[,] tablero { get; set; }
            bool EsMovimientoSeguro(int fila, int columna);
        }
    }
}
