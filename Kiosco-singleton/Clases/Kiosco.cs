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

        object locked = new();
        public void Comprar(IUsuario pCliente, IProducto pProducto)
        {
            if(pProducto == null) 
            {
                Console.WriteLine("No tenemos dicho producto");
                return;
            }

            if (pProducto.RequiereEdad && pCliente.Edad < 18)
            {
                Console.WriteLine($"{pCliente.Nombre}: No puedes comprar {pProducto.Nombre} porque eres menor de edad.");
            }

            else if (pProducto.esAlcohol && EnVeda())
            {
                Console.WriteLine($"{pCliente.Nombre}: No puedes comprar {pProducto.Nombre} debido a la veda electoral.");
            }

            else
            {
                lock (locked)
                {
                    if (pProducto.Stock <= 0)
                    {
                            Console.WriteLine($"{pCliente.Nombre}: {pProducto.Nombre} no está en stock.");
                    }
                    
                    else
                    {
                        lock (locked)
                        {
                            pProducto.DescontarStock();
                            Console.WriteLine($"{pCliente.Nombre}: Compraste {pProducto.Nombre}. Stock restante: {pProducto.Stock}");
                        }
                    }   
                } 
            }
        }
    }
}
