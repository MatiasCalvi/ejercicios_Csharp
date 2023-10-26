using Clases.Interfaces;

namespace Clases
{
    public class Billetera : IBilletera
    {
        public decimal Saldo { get; set; }
        public Billetera(decimal saldo)
        {
            Saldo = saldo;
        }
        public bool ValidarSaldo(Producto producto)
        {
            return Saldo >= producto.Precio;
        }

        public void ActualizarSaldo(Producto producto)
        {
            if (Saldo - producto.Precio >= 0)
            {
                Saldo -= producto.Precio;
            }

        }
    }

}
