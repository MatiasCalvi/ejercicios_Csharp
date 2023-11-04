using Clases.Interfaces;

public class Producto : IProducto
{
    public string producto_Nombre { get; set; }
    public int producto_Precio { get; set; }
    public int producto_Stock { get; set; }
    public bool producto_EsAlcohol { get; set; }
    public bool producto_RequiereEdad { get; set; }
    public Producto()
    {

    }
    public void DescontarStock()
    {
        producto_Stock--;
    }
}
