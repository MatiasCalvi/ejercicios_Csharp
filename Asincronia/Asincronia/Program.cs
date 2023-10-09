namespace Asincronia
{
    class Program
    {
        static async Task Main()
        {
            int[] arrayCompleto = new int[30000];
            Random random = new();
            
            for (int i = 0; i <= arrayCompleto.Length-1; i++)
            {   
                arrayCompleto[i]= random.Next(0, 45001);
            }

            for (int i=0; i<=arrayCompleto.Length-1; i++){
                Console.WriteLine("Array en Posicion {0} tiene el numero: '{1}'", i, arrayCompleto[i]);
            }

            int mayor = 0;
            int posicion = 0;
            object bloqueo = new();

            
            Task tarea1 = Task.Run(() =>
            {
                for(int i = 0; i <= arrayCompleto.Length/2; i++) {
                    lock(bloqueo)
                    {
                        if (arrayCompleto[i] > mayor)
                        {   
                            mayor = arrayCompleto[i];
                            posicion = i;
                        }
                    }
                }
            });

            Task tarea2 = Task.Run(() =>
            {
                for(int i = arrayCompleto.Length / 2; i <= arrayCompleto.Length-1; i++)
                {
                    lock(bloqueo)
                    {
                        if (arrayCompleto[i] > mayor)
                        {
                            mayor = arrayCompleto[i];
                            posicion = i;
                        }
                    }
                }
            });

            await Task.WhenAll(tarea1, tarea2);
            
            Console.WriteLine("El numero Mayor esta ubicado en la posicion {0} y es {1}",posicion,mayor);
        }

    }

}