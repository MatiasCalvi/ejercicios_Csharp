using System;
using System.Threading.Tasks;
using ArrayAsync;

class Program
{
    static async Task Main()
    {
        ArrayAsync.ArrayAsync metodos = new();

        int arrayLength = 100;
        int[] arrayCompleto = new int[arrayLength];

        metodos.Llenar_Imprimir(arrayCompleto, arrayLength);

        var tarea1 = metodos.Tarea1(arrayCompleto, arrayLength);
        var tarea2 = metodos.Tarea2(arrayCompleto, arrayLength);

        await Task.WhenAll(tarea1, tarea2);

        int mayor = 0;
        mayor = Math.Max(mayor, Math.Max(tarea1.Result, tarea2.Result));
        int indiceMayor = Array.IndexOf(arrayCompleto, mayor);


        Console.WriteLine("El número mayor es {0} en la posicion {1}", mayor, indiceMayor);
    }
}







