using System;
using System.Threading.Tasks;
using ArrayAsync;

class Program
{
    static async Task Main()
    {
        ArrayAsync.ArrayAsync metodos = new();

        int arrayLength = 30000;

        await metodos.IniciarPrograma(arrayLength);
    }

}







