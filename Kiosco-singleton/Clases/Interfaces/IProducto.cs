namespace Clases.Interfaces
{
    public interface IProducto
    {
        public string producto_Nombre { get;set; }
        public int producto_Precio { get; set; }
        public int producto_Stock { get; set; }
        public bool producto_EsAlcohol { get; set; }
        public bool producto_RequiereEdad { get; set; }
        public void DescontarStock();
    }
}
