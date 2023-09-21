
using Piezas.Piezas;

namespace TuEspacioDeNombres
{   
    class Program
    {   
  
        static void FuncionMostrar9(Reina pieza, NuevaPieza nuevaPieza)
        {
            if (pieza.Backtracking(0))
            {
                pieza.MostrarPosicion();

                for (int fila = 0; fila < 8; fila++)
                {
                    for (int columna = 0; columna < 8; columna++)
                    {
                        if (nuevaPieza.EsMovimientoSeguro(pieza.tablero, fila, columna))
                        {
                            Console.WriteLine("La NuevaPieza puede ser colocada en la fila {0}, columna {1}", fila, columna);
                            return;
                        }
                    }
                }

                Console.WriteLine("No se encontró una posición para la NuevaPieza.");
            }
            else
            {
                Console.WriteLine("No se encontró una solución.");
            }
        }

        static void Main(string[] args)
        {
            Reina pieza = new Reina(); // *---> Puede ser cualquier pieza dependiendo del constructor "Peon","Rey","Caballo","Torre","Reina","Alfil".
            NuevaPieza nuevaPieza = new NuevaPieza(); //*---> Nueva Pieza, se mueve en cualquier dialgonal pero no horizontal-vertical y solo una casilla a la vez


            FuncionMostrar9(pieza, nuevaPieza);
        }
    }
}