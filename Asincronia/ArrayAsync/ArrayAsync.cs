namespace ArrayAsync
{
    public class ArrayAsync
    {
        public int ProcesarElemento(int pInicio, int pFin, int[] pArray)
        {
            int mayorLocal = 0;

            for (int i = pInicio; i < pFin; i++)
            {
                if (pArray[i] > mayorLocal)
                {
                    mayorLocal = pArray[i];
                }
            }

            return mayorLocal;
        }

        public void Llenar_Imprimir(int[] pArrayCompleto, int pArrayLength)
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
    }
}