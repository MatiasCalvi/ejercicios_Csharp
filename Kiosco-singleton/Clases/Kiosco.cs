using Clases.Interfaces;
using Clases.Exceptions;

namespace Clases
{
    public class Kiosco : IKiosco
    {
        private static readonly Kiosco instance = new();
        private List<Producto> Productos;
        private Kiosco()
        {
            Productos = new List<Producto>();
        }

        public static Kiosco Instance
        {
            get
            {
                return instance;
            }
        }

        public void CargarProductosDesdeBaseDeDatos()
        {
            var dao = new DaoLista();
            Productos = dao.ObtenerTodosLosProductos();
        }

        public void MostrarProductos()
        {
            Console.WriteLine("Productos:");
            foreach (var producto in Productos)
            {
                Console.WriteLine($"{producto.producto_Nombre} - Precio: ${producto.producto_Precio} - Stock: {producto.producto_Stock}");
            }
        }

        public List<Producto> ListaProductos()
        {
            return Productos;
        }
        public Producto BuscarProducto(string pNombre)
        {
            foreach (Producto produc in Productos)
            {
                if (produc.producto_Nombre == pNombre)
                {
                    return produc;
                }
            }
            return null;
        }

        public static bool EnVeda()
        {
            return false; //---> cambialo si queres consumir alcohol
        }

        object locked = new();
        public void Comprar(IUsuario pCliente, Producto pProducto)
        {
            lock (locked)
            {
                try
                {
                    if (pProducto == null)
                    {
                        throw new NoHayProductoException();
                    }

                    if (pProducto.producto_RequiereEdad && pCliente.Edad < 18)
                    {
                        throw new EresMenorException(pCliente.Nombre, pProducto.producto_Nombre);
                    }

                    else if (pProducto.producto_EsAlcohol && EnVeda())
                    {
                        throw new VedaElectoralException(pCliente.Nombre, pProducto.producto_Nombre);
                    }

                    else
                    {
                        if (pProducto.producto_Stock <= 0)
                        {
                            throw new NoHayStockException(pCliente.Nombre, pProducto.producto_Nombre);
                        }

                        else
                        {
                            lock (locked)
                            {
                                pProducto.DescontarStock();
                                var dao = new DaoLista();
                                dao.ActualizarStock(pProducto);
                                Console.WriteLine($"{pCliente.Nombre}: Compraste {pProducto.producto_Nombre}. Stock restante: {pProducto.producto_Stock}");
                            }
                        }
                    }
                }
                catch (EresMenorException e)
                {
                    Console.WriteLine($"{pCliente.Nombre}: {e.Message}");
                }
                catch (VedaElectoralException e)
                {
                    Console.WriteLine($"{pCliente.Nombre}: {e.Message}");
                }
                catch (NoHayStockException e)
                {
                    Console.WriteLine($"{pCliente.Nombre}: {e.Message}");
                }
                catch (NoHayProductoException e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
