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
        if (tablero[fila, i] == Estados.Ocupada || tablero[i, columna] == Estados.Ocupada) // --> Horizontal y Vertical
            return false;
    }

    for (int i = 0; i < 8; i++)
    {
        if (fila - i >= 0 && columna - i >= 0 && tablero[fila - i, columna - i] == Estados.Ocupada) // --> Diagonal hacia arriba-izquierda
            return false;

        if (fila - i >= 0 && columna + i < 8 && tablero[fila - i, columna + i] == Estados.Ocupada) // --> Diagonal hacia arriba-derecha
            return false;

        if (fila + i < 8 && columna - i >= 0 && tablero[fila + i, columna - i] == Estados.Ocupada) // --> Diagonal hacia abajo-izquierda
            return false;

        if (fila + i < 8 && columna + i < 8 && tablero[fila + i, columna + i] == Estados.Ocupada) // --> Diagonal hacia abajo-derecha
            return false;
    }

    return true;

}


bool ColocarReinas(Estados[,] tablero, int columna)
{
    if (columna >= 8)
    {
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
    Console.WriteLine("Solución encontrada:");

    Console.WriteLine("Número de reinas colocadas: 8");

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
}
else
{
    Console.WriteLine("No se encontró una solución.");
}

enum Estados
{
    Libre = 0,
    Ocupada = 1,
}
