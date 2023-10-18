using System.Threading;

namespace ArrayAsync
{
    public class ArrayAsync
    {
        private readonly object arrayLock1 = new();
        private readonly object arrayLock2 = new();

        public int ProcesarElemento(int pInicio, int pFin, int[] pArray)
        {
            int mayorLocal = 0;

            for (int i = pInicio; i < pFin ; i++)
            {
                lock (arrayLock1)
                {
                    if (pArray[i] > mayorLocal)
                    {
                        lock (arrayLock2)
                        {
                            mayorLocal = pArray[i];
                        }
                    }
                }
            }

            return mayorLocal;
        }

        public static void Llenar_Imprimir(int[] pArrayCompleto, int pArrayLength)
        {
            Random random = new();

            for (int i = 0; i < pArrayLength; i++)
            {
                pArrayCompleto[i] = random.Next(0, 41001);
                Console.WriteLine("Posicion {0} con el valor {1}", i, pArrayCompleto[i]);
            }

        }

        public Task<int> Tarea1(int[] pArrayCompleto, int pArrayLength)
        {
            return Task.Run(() =>
            {
                int mitad = pArrayLength / 2;
                return ProcesarElemento(0, mitad, pArrayCompleto);
            });
        }

        public Task<int> Tarea2(int[] pArrayCompleto, int pArrayLength)
        {
            return Task.Run(() =>
            {
                int mitad = pArrayLength / 2;
                return ProcesarElemento(mitad, pArrayLength, pArrayCompleto);
            });
        }

        public async Task<(int, int)> EjecutarTareas(int[] pArrayCompleto, int pArrayLength)
        {
            var tarea1 = Tarea1(pArrayCompleto, pArrayLength);
            var tarea2 = Tarea2(pArrayCompleto, pArrayLength);

            await Task.WhenAll(tarea1, tarea2);

            int mayor = 0;
            mayor = Math.Max(mayor, Math.Max(tarea1.Result, tarea2.Result));
            int indiceMayor = Array.IndexOf(pArrayCompleto, mayor);

            return (mayor, indiceMayor);
        }
        public async Task IniciarPrograma(int pArrayLength)
        {
            int[] arrayCompleto = new int[pArrayLength]; 

            Llenar_Imprimir(arrayCompleto, pArrayLength); 

            var (mayor, indiceMayor) = await EjecutarTareas(arrayCompleto, pArrayLength); 

            Console.WriteLine("El número mayor es {0} en la posicion {1}", mayor, indiceMayor); 
        }
    }
}