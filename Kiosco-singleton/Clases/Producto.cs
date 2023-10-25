using Clases.Interfaces;

public class Producto : IProducto
{
    public string Nombre { get; set; }
    public int Precio { get; set; }
    public int Stock { get; set; }
    public bool esAlcohol { get; set; }
    public bool RequiereEdad { get; set; }

    public Producto(string nombre, int precio, int stock, bool pEsAlcohol, bool requiereEdad)
    {
        Nombre = nombre;
        Precio = precio;
        Stock = stock;
        esAlcohol = pEsAlcohol;
        RequiereEdad = requiereEdad;
    }

    public void DescontarStock()
    {
        Stock--;
    }
}


