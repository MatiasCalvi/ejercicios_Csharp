using Clases;

namespace Kiosco{
    class Program
    {
        static async Task Main()
        {
            var kiosco = Clases.Kiosco.Instance;
            var cliente1 = new Usuario("Cliente1", 20);
            var cliente2 = new Usuario("Cliente2", 18);
            var producto1 = kiosco.BuscarProducto("Brahma"); 
            var producto2 = kiosco.BuscarProducto("Brahma");

            var tasks = new List<Task>();

            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Run(() => kiosco.Comprar(cliente1, producto1)));
                tasks.Add(Task.Run(() => kiosco.Comprar(cliente2, producto2)));
                //tasks.Add(Task.Run(() => kiosco.Comprar(cliente1, kiosco.BuscarProducto("Galletitas"))));
                //tasks.Add(Task.Run(() => kiosco.Comprar(new Usuario("Cliente3",20), kiosco.BuscarProducto("Galletitas"))));
            }
            
            await Task.WhenAll(tasks);
        }
    }
}
