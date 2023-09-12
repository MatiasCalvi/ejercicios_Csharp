Estados[,] tablero = new Estados[8, 8];

for (int i = 0; i < 8; i++)
{
    for (int j = 0; j < 8; j++)
    {
        tablero[i, j] = Estados.Libre;
    }
}

bool EsSeguroColocarReina(Estados[,] tablero, int fila, int columna)
{
    for (int i = 0; i < 8; i++)
    {
        if (tablero[fila, i] == Estados.Ocupada)
            return false;
    }

    for (int i = 0; i < 8; i++)
    {
        if (tablero[i, columna] == Estados.Ocupada)
            return false;
    }

    for (int i = fila, j = columna; i >= 0 && j >= 0; i--, j--)
    {
        if (tablero[i, j] == Estados.Ocupada)
            return false;
    }

    for (int i = fila, j = columna; i >= 0 && j < 8; i--, j++)
    {
        if (tablero[i, j] == Estados.Ocupada)
            return false;
    }

    for (int i = fila, j = columna; i < 8 && j >= 0; i++, j--)
    {
        if (tablero[i, j] == Estados.Ocupada)
            return false;
    }

    for (int i = fila, j = columna; i < 8 && j < 8; i++, j++)
    {
        if (tablero[i, j] == Estados.Ocupada)
            return false;
    }

    return true;
}


bool ColocarReinas(Estados[,] tablero, int columna)
{
    if (columna >= 8)
    {
        Console.WriteLine("Solución encontrada:");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (tablero[i, j] == Estados.Ocupada)
                {
                    Console.WriteLine($"Reina en fila {i}, columna {j}");
                }
            }
        }
        return true;
    }

    for (int fila = 0; fila < 8; fila++)
    {
        if (EsSeguroColocarReina(tablero, fila, columna))
        {
            tablero[fila, columna] = Estados.Ocupada;

            if (ColocarReinas(tablero, columna + 1))
                return true;

            tablero[fila, columna] = Estados.Libre;
        }
    }

    return false;
}

if (ColocarReinas(tablero, 0))
{
    Console.WriteLine("Número de reinas colocadas: 8");
}
else
{
    Console.WriteLine("No se encontró una solución.");
}

enum Estados
{
    Libre = 0,
    Ocupada = 1,
    Marcada = 2
}
