using Clases.Interfaces;

namespace Clases
{
    public class Kiosco : IKiosco
    {
        private static readonly Kiosco instance = new();
        private List<IProducto> productos;
        private Kiosco()
        {
            productos = new List<IProducto>();
            productos.Add(new Producto("Coca-Cola", 500, 10, false, false));
            productos.Add(new Producto("Chocolate", 400, 5, false, false));
            productos.Add(new Producto("Galletitas", 300, 8, false, false));
            productos.Add(new Producto("Brahma", 450, 6, true, true));
            productos.Add(new Producto("Quilmes", 550, 7, true, true));
        }

        public static Kiosco Instance
        {
            get
            {
                return instance;
            }
        }

        public void MostrarProductos()
        {
            Console.WriteLine("Productos:");
            foreach (var producto in this.productos)
            {
                Console.WriteLine($"{producto.Nombre} - Precio: ${producto.Precio} - Stock: {producto.Stock}");
            }
        }

        public List<IProducto>Productos()
        {
            return this.productos;
        }
        public Producto BuscarProducto(string pNombre)
        {
            foreach(Producto produc in productos)
            {
                if(produc.Nombre == pNombre)
                {
                    return produc;
                }
            }
            return null;
        }

        public static bool EnVeda()
        {
            return true; //---> cambialo si queres consumir alcohol
        }

        public void Comprar(IUsuario cliente, IProducto producto)
        {

                if (producto.RequiereEdad && cliente.Edad < 18)
                {
                    Console.WriteLine($"{cliente.Nombre}: No puedes comprar {producto.Nombre} porque eres menor de edad.");
                }
                else if (producto.esAlcohol && EnVeda())
                {
                    Console.WriteLine($"{cliente.Nombre}: No puedes comprar {producto.Nombre} debido a la veda electoral.");
                }
                else
                {
                    lock (producto)
                    {
                        if (producto.Stock <= 0)
                        {
                            Console.WriteLine($"{cliente.Nombre}: {producto.Nombre} no está en stock.");
                        }
                        else {
                            producto.DescontarStock();
                            Console.WriteLine($"{cliente.Nombre}: Compraste {producto.Nombre}. Stock restante: {producto.Stock}");
                        }
                    }
                }
        }

    }
}

