using static Piezas.IPieza;

namespace Piezas.Piezas
{
    public class NuevaPieza 
    {
        public bool EsMovimientoSeguro(Casilla[,] tablero, int fila, int columna, IPieza pieza)
        {
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
                            if(!esAtacada(nuevaFila,nuevaColumna,pieza))
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    
        public bool esAtacada(int fila, int columna,IPieza pieza)
        {
             if (pieza.EsMovimientoSeguro(fila, columna))
             {
                return true;
             }
            
            return false;
        }
    }
}
