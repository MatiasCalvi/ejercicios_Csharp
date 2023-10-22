using Piezas;
using Piezas.Piezas;

namespace TuEspacioDeNombres
{
    class Program
    {

        static void FuncionMostrar9(Caballo pieza, NuevaPieza nuevaPieza)
        {
            if (pieza.Backtracking(0))
            {
                pieza.MostrarPosicion();

                bool hayLugar = false;
                for (int i = 0; i < 8; i++)
                {

                    for (int j = 0; j < 8; j++)
                    {
                        if (pieza.Tablero[i, j] == IPieza.Casilla.Libre)
                        {
                            if (nuevaPieza.EsMovimientoSeguro(pieza.Tablero, i, j, pieza))
                            {
                                Console.WriteLine("NuevaPieza en fila {0}, columna {1}", i, j);
                                hayLugar = true;
                                break; 
                            }
                        }
                    }

                    if (hayLugar) break; 
                }

                if (!hayLugar) 
                {
                    Console.WriteLine("No hay lugar para la nuevaPieza");
                }
            }
        }

        static void Main()
        {
            IPieza pieza = new Caballo();
            NuevaPieza nuevaPieza = new();

            IPieza piezaT = new PiezaT();

            try
            {
                FuncionMostrar9((Caballo)pieza, nuevaPieza);
            }
            catch (SinSolucionException e) 
            {
                Console.WriteLine(e.Message); 
                Console.WriteLine(e.StackTrace); 
            }

        }
    }
}





















