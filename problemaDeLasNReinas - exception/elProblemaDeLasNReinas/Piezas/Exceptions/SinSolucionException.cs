public class SinSolucionException : Exception
{
    public SinSolucionException() : base("No se encontró solución al problema de las piezas")
    {

    }
    public SinSolucionException(string message) : base(message)
    {

    }
}



