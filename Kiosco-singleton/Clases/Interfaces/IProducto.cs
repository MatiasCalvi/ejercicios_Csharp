namespace Clases.Interfaces
{
    public interface IProducto
    {
        public string Nombre{ get;set; }
        public int Precio{ get; set; }
        public int Stock{ get; set; }
        public bool esAlcohol { get; set; }
        public bool RequiereEdad { get; set; }
        public void DescontarStock();
    }
}
